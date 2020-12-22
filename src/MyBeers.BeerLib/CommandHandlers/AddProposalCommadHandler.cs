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
using System.Threading.Tasks;

namespace MyBeers.BeerLib.CommandHandlers
{
    public class AddProposalCommadHandler : BaseCommandHandler<AddProposalCommand, Domain.BeerChangeRequest>
    {
        private readonly IAmazonS3 _amazonS3;
        private readonly IUserService _userService;

        public AddProposalCommadHandler(IMongoRepository<BeerChangeRequest> repository, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher, IAmazonS3 amazonS3, IUserService userService) : base(repository, queryDispatcher, commandDispatcher)
        {
            _amazonS3 = amazonS3;
            _userService = userService;
        }

        public override async Task HandleAsync(AddProposalCommand command)
        {
            var oldBeer = await QueryDispatcher.DispatchAsync<BeerQuery, BeerQuery.Beer>(new BeerQuery { Id = command.BeerId });

            var newBeer = MakeBeer(command);

            var imageUrl = command.BeerData.Image;
            if (imageUrl != oldBeer.ImageUrl)
            {
                newBeer.ImageUrl = await UploadNewImage(imageUrl);
            }

            var userId = _userService.GetUserId();
            var beerChange = new BeerChangeRequest
            {
                Status = Status.New,
                UserId = userId,
                DateCreated = DateTime.UtcNow,
                NewBeerInfo = newBeer,
                OldBeerInfo = MakeOldBeer(oldBeer)
            };

            await Repository.SaveAsync(beerChange);
        }

        private async Task<string> UploadNewImage(string imageUrl)
        {
            if (imageUrl.StartsWith("http"))
            {
                throw new Exception("Image not ok");
            }

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
                        pic.Quality = 60;

                        pic.Write(fileToUpload);
                    }

                    await fileTransferUtil.UploadAsync(fileToUpload, bucketName, fileName);

                    var image = $"https://mybeers-beerimages.s3.eu-north-1.amazonaws.com/{fileName}";

                    fileToUpload.Close();

                    return image;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private BeerRequestModel MakeBeer(AddProposalCommand command)
        {
            return new BeerRequestModel
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
                Containers = command.BeerData.Containers.Select(c => 
                new ContainerModel
                {
                    Id = c.Id,
                    Type = c.Type,
                    Volume = c.Volume,
                    Price = c.Price,
                    RecycleFee = c.RecycleFee,
                    Ypk = c.Ypk,
                    SellStartDate = c.SellStartDate,
                    ProductIdFromSystmet = c.ProductIdFromSystemet
                })
            };
        }


        private BeerRequestModel MakeOldBeer(BeerQuery.Beer beer)
        {
            return new BeerRequestModel
            {
                Id = beer.Id.ToString(),
                AlcoholPercentage = beer.AlcoholPercentage,
                City = beer.City,
                Country = beer.Country,
                ImageUrl = beer.ImageUrl,
                Name = beer.Name,
                Producer = beer.Producer,
                State = beer.State,
                Style = beer.Style,
                Type = beer.Type,
                Containers = beer.Containers.Select(c => new ContainerModel
                {
                    Id = c.Id,
                    Type = c.Type,
                    Volume = c.Volume,
                    Price = c.Price,
                    RecycleFee = c.RecycleFee,
                    Ypk = c.Ypk,
                    SellStartDate = c.SellStartDate,
                    ProductIdFromSystmet = c.ProductIdFromSystemet
                })
            };
        }

        private bool CompareProperty<T>(T newProp, T oldProp)
        {
            return EqualityComparer<T>.Default.Equals(oldProp, newProp);
        }
    }
}
