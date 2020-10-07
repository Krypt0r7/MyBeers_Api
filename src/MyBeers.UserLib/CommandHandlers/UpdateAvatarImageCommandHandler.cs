using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using ImageMagick;
using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using MyBeers.Common.Services;
using MyBeers.UserLib.Api.Commands;
using MyBeers.UserLib.Domain;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MyBeers.UserLib.CommandHandlers
{
    public class UpdateAvatarImageCommandHandler : BaseCommandHandler<UpdateAvatarImageCommand, Domain.User>
    {
        private readonly IAmazonS3 _s3client;
        private readonly IUserService userService;

        public UpdateAvatarImageCommandHandler(IMongoRepository<User> repository, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher, IAmazonS3 amazonS3, IUserService userService) : base(repository, queryDispatcher, commandDispatcher)
        {
            _s3client = amazonS3;
            this.userService = userService;
        }

        public override async Task HandleAsync(UpdateAvatarImageCommand command)
        {
            var user = await Repository.FindByIdAsync(userService.GetUserId());

            var imageDataByteArray = Convert.FromBase64String(command.ImageData);
            string bucketName = "mybeers-avatars";

            try
            {
                if (await AmazonS3Util.DoesS3BucketExistV2Async(_s3client, bucketName) == false)
                {
                    var putBucket = new PutBucketRequest
                    {
                        BucketName = bucketName,
                        UseClientRegion = true
                    };
                    var response = await _s3client.PutBucketAsync(putBucket);
                }

                var fileTransferUtil = new TransferUtility(_s3client);

                string fileName = Guid.NewGuid().ToString() + ".png";

                using (var fileToUpload = new MemoryStream())
                {
                    fileToUpload.Position = 0;
                    using (var pic = new MagickImage(imageDataByteArray))
                    {
                        pic.Resize(250, 250);
                        pic.Format = MagickFormat.Png;
                        pic.Quality = 70;

                        pic.Write(fileToUpload);
                    }

                    await fileTransferUtil.UploadAsync(fileToUpload, bucketName, fileName);

                    user.AvatarUrl = $"https://mybeers-avatars.s3.eu-north-1.amazonaws.com/{fileName}";

                    await Repository.ReplaceAsync(user);

                    fileToUpload.Close();
                }

            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
