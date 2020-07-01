using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using MyBeers.ListLib.Api.Commands;
using MyBeers.ListLib.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyBeers.ListLib.CommandHandlers
{
    public class DeleteListCommandHandler : BaseCommandHandler<DeleteListCommand, Domain.List>
    {
        public DeleteListCommandHandler(IMongoRepository<List> repository, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher) : base(repository, queryDispatcher, commandDispatcher)
        {
        }

        public override async Task HandleAsync(DeleteListCommand command)
        {
            var list = Repository.FindByIdAsync(command.Id);
            if (list == null)
                throw new Exception($"Not found {command.Id}");

            await Repository.DeleteAsync(command.Id);
        }
    }
}
