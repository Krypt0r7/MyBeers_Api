using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using MyBeers.UserLib.Api.Commands;
using MyBeers.UserLib.Domain;
using MyBeers.UserLib.Helpers;
using System;
using System.Threading.Tasks;

namespace MyBeers.UserLib.CommandHandlers
{
    public class UpdatePasswordCommandHandler : BaseCommandHandler<UpdatePasswordCommand, Domain.User>
    {
        public UpdatePasswordCommandHandler(IMongoRepository<User> repository, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher) : base(repository, queryDispatcher, commandDispatcher)
        {
        }

        public override async Task HandleAsync(UpdatePasswordCommand command)
        {
            var user = await Repository.FindByIdAsync(command.Id);
            if (user == null)
                throw new Exception("User not found");

            byte[] passwordHash, passwordSalt;
            PasswordHelpers.CreatePasswordHash(command.Password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await Repository.ReplaceAsync(user);

        }
    }
}
