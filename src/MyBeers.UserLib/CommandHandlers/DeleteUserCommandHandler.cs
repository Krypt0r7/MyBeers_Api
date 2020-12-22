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
    public class DeleteUserCommandHandler : BaseCommandHandler<DeleteUserCommand, Domain.User>
    {
        public DeleteUserCommandHandler(IMongoRepository<User> repository, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher) : base(repository, queryDispatcher, commandDispatcher)
        {
        }

        public async override Task HandleAsync(DeleteUserCommand command)
        {
            var user = Repository.FindByIdAsync(command.Id);
            if (user != null)
            {
                await Repository.DeleteAsync(command.Id);
            }
            else
            {
                throw new UserException($"User with id {command.Id} not found");
            }
        }
    }
}
