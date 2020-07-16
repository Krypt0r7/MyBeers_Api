using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using MyBeers.ListLib.Api.Commands;
using MyBeers.ListLib.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBeers.ListLib.CommandHandlers
{
    public class UpdateListInfoCommandHandler : BaseCommandHandler<UpdateListInfoCommand, Domain.List>
    {
        public UpdateListInfoCommandHandler(IMongoRepository<List> repository, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher) : base(repository, queryDispatcher, commandDispatcher)
        {
        }

        public override async Task HandleAsync(UpdateListInfoCommand command)
        {
            var list = await Repository.FindByIdAsync(command.Id);

            if (list == null)
                throw new Exception($"NOt found {command.Id}");

            list.Name = command.Name;
            list.Description = command.Description;
            list.Collaborators = command.CollaboratorIds.ToList();

            await Repository.ReplaceAsync(list);
        }
    }
}
