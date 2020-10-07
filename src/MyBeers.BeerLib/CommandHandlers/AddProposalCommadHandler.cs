using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using ImageMagick;
using MyBeers.BeerLib.Api.Commands;
using MyBeers.BeerLib.Api.Queries;
using MyBeers.BeerLib.Domain;
using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using MyBeers.Common.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MyBeers.BeerLib.CommandHandlers
{
    public class AddProposalCommadHandler : BaseCommandHandler<AddProposalCommand, Domain.BeerChangeRequest>
    {
        private List<BeerChangeRequest.Change> changedBeerData = new List<BeerChangeRequest.Change>();
        private readonly IAmazonS3 _amazonS3;
        private readonly IUserService userService;

        public AddProposalCommadHandler(IMongoRepository<BeerChangeRequest> repository, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher, IAmazonS3 amazonS3, IUserService userService) : base(repository, queryDispatcher, commandDispatcher)
        {
            _amazonS3 = amazonS3;
            this.userService = userService;
        }

        public override async Task HandleAsync(AddProposalCommand command)
        {
            var oldBeer = await QueryDispatcher.DispatchAsync<BeerQuery, BeerQuery.Beer>(new BeerQuery { Id = command.BeerId });

            var newBeer = MakeBeer(command);

            var fi = newBeer.GetType().GetProperties();

            foreach (var field in fi)
            {
                if (field.Name == "Containers")
                {
                    var oldContainers = (IEnumerable<BeerQuery.Beer.Container>)field.GetValue(oldBeer, null);
                    var newContainers = (IEnumerable<BeerQuery.Beer.Container>)field.GetValue(newBeer, null);

                    if (newContainers.Count() == oldContainers.Count())
                    {
                        for (int i = 0; i < oldContainers.Count(); i++)
                        {
                            var containerProperties = oldContainers.ToArray()[i].GetType().GetProperties();
                            foreach (var prop in containerProperties)
                            {
                                ControllAndAddChange(oldContainers.ToArray()[i], newContainers.ToArray()[i], prop);
                            }
                        }
                    }
                }
                else if (field.Name == "MoreInformationModel")
                {
                    var oldInformationModel = field.GetValue(oldBeer);
                    var newInformationModel = field.GetValue(newBeer);

                    var informationModelFields = oldInformationModel.GetType().GetProperties();

                    foreach (var infoField in informationModelFields)
                    {
                        ControllAndAddChange(oldInformationModel, newInformationModel, infoField);
                    }

                }
                else if (field.Name == "ImageUrl")
                {
                    var isEqual = CompareProperty(field.GetValue(oldBeer), field.GetValue(newBeer));
                    if (!isEqual)
                    {
                        var imageUrl = field.GetValue(newBeer).ToString();
                        var imageData = imageUrl.Remove(0, imageUrl.IndexOf(',') + 1);
                        var imageDataByteArray = Convert.FromBase64String(imageData);
                        string bucketName = "mybeers-beerimages";

                        try
                        {
                            if (await AmazonS3Util.DoesS3BucketExistV2Async(_amazonS3, bucketName) == false)
                            {
                                var putBucket = new PutBucketRequest
                                {
                                    BucketName = bucketName,
                                    UseClientRegion = true
                                };
                                var response = await _amazonS3.PutBucketAsync(putBucket);
                            }

                            var fileTransferUtil = new TransferUtility(_amazonS3);

                            string fileName = Guid.NewGuid().ToString() + ".png";

                            using (var fileToUpload = new MemoryStream())
                            {
                                fileToUpload.Position = 0;
                                using (var pic = new MagickImage(imageDataByteArray))
                                {
                                    pic.Resize(500, 500);
                                    pic.Format = MagickFormat.Png;
                                    pic.Quality = 70;

                                    pic.Write(fileToUpload);
                                }

                                await fileTransferUtil.UploadAsync(fileToUpload, bucketName, fileName);

                                var image = $"https://mybeers-beerimages.s3.eu-north-1.amazonaws.com/{fileName}";

                                fileToUpload.Close();

                                var change = new BeerChangeRequest.Change
                                {
                                    NewValue = image,
                                    OldValue = field.GetValue(oldBeer) ?? "",
                                    Property = field.Name
                                };

                                changedBeerData.Add(change);
                            }

                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                    }
                }
                else
                {
                    ControllAndAddChange(oldBeer, newBeer, field);
                }

            }

            if (changedBeerData.Count > 0)
            {
                var userId = userService.GetUserId();
                var beerChange = new BeerChangeRequest
                {
                    BeerId = newBeer.Id,
                    Changes = changedBeerData,
                    ChangeStatus = BeerChangeRequest.Status.New,
                    UserId = userId,
                    DateCreated = DateTime.UtcNow
                };

                await Repository.SaveAsync(beerChange);
            }
        }

        private void ControllAndAddChange<T>(T oldBeer, T newBeer, PropertyInfo field)
        {
            var change = new BeerChangeRequest.Change
            {
                NewValue = field.GetValue(newBeer) ?? "",
                OldValue = field.GetValue(oldBeer) ?? "",
                Property = field.Name
            };

            if (!change.NewValue.Equals(change.OldValue))
            {
                changedBeerData.Add(change);
            }
        }

        private BeerQuery.Beer MakeBeer(AddProposalCommand command)
        {
            return new BeerQuery.Beer
            {
                Id = command.BeerData.Id.ToString(),
                AlcoholPercentage = command.BeerData.AlcoholPercentage,
                City = command.BeerData.City,
                Country = command.BeerData.Country,
                ImageUrl = command.BeerData.Image,
                Name = command.BeerData.Name,
                Producer = command.BeerData.Producer,
                State = command.BeerData.State,
                Style = command.BeerData.Style,
                Type = command.BeerData.Type,
                Containers = command.BeerData.Containers.Select(c => new BeerQuery.Beer.Container
                {
                    Price = c.Price,
                    ProductIdFromSystemet = c.ProductIdFromSystemet,
                    ProductionScale = c.ProductionScale,
                    RecycleFee = c.RecycleFee,
                    SellStartDate = c.SellStartDate,
                    Type = c.Type,
                    Volume = c.Volume,
                    Ypk = c.Ypk
                }),
                MoreInformationModel = new BeerQuery.Beer.MoreInformation
                {
                    BeverageDescriptionShort = command.BeerData.MoreInformationModel.BeverageDescriptionShort,
                    AssortmentText = command.BeerData.MoreInformationModel.AssortmentText,
                    Assortment = command.BeerData.MoreInformationModel.Assortment,
                    AlcoholPercentage = command.BeerData.MoreInformationModel.AlcoholPercentage,
                    Category = command.BeerData.MoreInformationModel.Category,
                    Country = command.BeerData.MoreInformationModel.Country,
                    IsNews = command.BeerData.MoreInformationModel.IsNews,
                    IsOrganic = command.BeerData.MoreInformationModel.IsOrganic,
                    OriginLevel1 = command.BeerData.MoreInformationModel.OriginLevel1,
                    OriginLevel2 = command.BeerData.MoreInformationModel.OriginLevel2,
                    ProducerName = command.BeerData.MoreInformationModel.ProducerName,
                    ProductId = command.BeerData.MoreInformationModel.ProductId,
                    ProductNameBold = command.BeerData.MoreInformationModel.ProductNameBold,
                    ProductNameThin = command.BeerData.MoreInformationModel.ProductNameThin,
                    ProductNumber = command.BeerData.MoreInformationModel.ProductNumber,
                    ProductNumberShort = command.BeerData.MoreInformationModel.ProductNumberShort,
                    Style = command.BeerData.MoreInformationModel.Style,
                    SubCategory = command.BeerData.MoreInformationModel.SubCategory,
                    SupplierName = command.BeerData.MoreInformationModel.SupplierName,
                    Taste = command.BeerData.MoreInformationModel.Taste,
                    Type = command.BeerData.MoreInformationModel.Type,
                    Usage = command.BeerData.MoreInformationModel.Usage,

                    Vintage = command.BeerData.MoreInformationModel.Vintage,
                }
            };
        }

        private bool CompareProperty<T>(T newProp, T oldProp) 
        {
            return EqualityComparer<T>.Default.Equals(oldProp, newProp);
        }
    }
}
