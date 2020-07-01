using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using MyBeers.UserLib.Api.Commands;
using MyBeers.UserLib.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyBeers.UserLib.CommandHandlers
{
    public class UpdateUserCommandHandler : BaseCommandHandler<UpdateUserCommand, Domain.User>
    {
        public UpdateUserCommandHandler(IMongoRepository<User> repository, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher) : base(repository, queryDispatcher, commandDispatcher)
        {
        }

        public override async Task HandleAsync(UpdateUserCommand command)
        {
            var user = await Repository.FindByIdAsync(command.Id);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.Email = command.Email;
            user.Username = command.Username;

            await Repository.ReplaceAsync(user);
        }
    }
}
