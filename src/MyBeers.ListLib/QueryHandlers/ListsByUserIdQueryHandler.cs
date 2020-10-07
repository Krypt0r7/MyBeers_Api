using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using MyBeers.ListLib.Domain;
using MyBeers.ListLib.Api.Queries;
using System.Linq;
using System.Threading.Tasks;
using MyBeers.Common.Services;

namespace MyBeers.ListLib.QueryHandlers
{
    public class ListsByUserIdQueryHandler : BaseQueryHandler<Domain.List, ListsByUserIdQuery, ListsByUserIdQuery.Lists>
    {
        private readonly IUserService userService;

        public ListsByUserIdQueryHandler(IMongoRepository<List> repository, IQueryDispatcher queryDispatcher, IUserService userService) : base(repository, queryDispatcher)
        {
            this.userService = userService;
        }

        public override async Task<ListsByUserIdQuery.Lists> HandleAsync(ListsByUserIdQuery query)
        {
            var userId = userService.GetUserId();
            var myLists = await Repository.FilterByAsync(filter => filter.OwnerId == userId);
            var collabLists = await Repository.FilterByAsync(f => f.Collaborators.Contains(userId));

            return new ListsByUserIdQuery.Lists
            {
                MyLists = myLists.Select(x => new ListsByUserIdQuery.List { Description = x.Description, Id = x.Id.ToString(), Name = x.Name}),
                CollaborateLists = collabLists.Select(x => new ListsByUserIdQuery.List { Description = x.Description, Id = x.Id.ToString(), Name = x.Name})
            };

        }
    }
}
