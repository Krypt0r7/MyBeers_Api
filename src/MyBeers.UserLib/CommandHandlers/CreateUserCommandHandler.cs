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
    public class CreateUserCommandHandler : BaseCommandHandler<CreateUserCommand, Domain.User>
    {
        public CreateUserCommandHandler(IMongoRepository<User> repository, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher) : base(repository, queryDispatcher, commandDispatcher)
        {
        }

        public override async Task HandleAsync(CreateUserCommand command)
        {
            var usertest = await Repository.FindOneAsync(filter => filter.Username == command.Username);
            if (usertest != null)
                throw new Exception("User already exists");

            byte[] passwordHash, passwordSalt;
            PasswordHelpers.CreatePasswordHash(command.Password, out passwordHash, out passwordSalt);

            var user = new Domain.User { Email = command.Email, Username = command.Username, PasswordHash = passwordHash, PasswordSalt = passwordSalt };
            await Repository.SaveAsync(user);
        }
    }
}
