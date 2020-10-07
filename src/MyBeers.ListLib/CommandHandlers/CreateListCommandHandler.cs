using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using MyBeers.Common.Services;
using MyBeers.ListLib.Api.Commands;
using MyBeers.ListLib.Domain;
using System.Threading.Tasks;

namespace MyBeers.ListLib.CommandHandlers
{
    public class CreateListCommandHandler : BaseCommandHandler<CreateListCommand, Domain.List>
    {
        private readonly IUserService userService;

        public CreateListCommandHandler(IMongoRepository<List> repository, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher, IUserService userService) : base(repository, queryDispatcher, commandDispatcher)
        {
            this.userService = userService;
        }

        public override async Task HandleAsync(CreateListCommand command)
        {
            var userId = userService.GetUserId();
            var list = new List { OwnerId =userId, Name = command.Name, Description = command.Description };

            await Repository.SaveAsync(list);
        }
    }
}
