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
    public class UpdateCollaboratorsCommandHandler : BaseCommandHandler<UpdateCollaboratorsCommand, Domain.List>
    {
        public UpdateCollaboratorsCommandHandler(IMongoRepository<List> repository, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher) : base(repository, queryDispatcher, commandDispatcher)
        {
        }

        public override async Task HandleAsync(UpdateCollaboratorsCommand command)
        {
            var list = await Repository.FindByIdAsync(command.Id);

            list.Collaborators = command.UserIds.ToList();

            await Repository.ReplaceAsync(list);
        }
    }
}
