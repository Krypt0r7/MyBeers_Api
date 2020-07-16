using MyBeers.BeerLib.Api.Queries;
using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using MyBeers.ListLib.Domain;
using MyBeers.ListLib.Api.Queries;
using MyBeers.UserLib.Api.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeers.ListLib.QueryHandlers
{
    public class ListsByUserIdQueryHandler : BaseQueryHandler<Domain.List, ListsByUserIdQuery, ListsByUserIdQuery.Lists>
    {
        public ListsByUserIdQueryHandler(IMongoRepository<List> repository, IQueryDispatcher queryDispatcher) : base(repository, queryDispatcher)
        {
        }

        public override async Task<ListsByUserIdQuery.Lists> HandleAsync(ListsByUserIdQuery query)
        {
            var myLists = await Repository.FilterByAsync(filter => filter.OwnerId == query.UserId);
            var collabLists = await Repository.FilterByAsync(f => f.Collaborators.Contains(query.UserId));

            return new ListsByUserIdQuery.Lists
            {
                MyLists = myLists.Select(x => new ListsByUserIdQuery.List { Description = x.Description, Id = x.Id.ToString(), Name = x.Name}),
                CollaborateLists = collabLists.Select(x => new ListsByUserIdQuery.List { Description = x.Description, Id = x.Id.ToString(), Name = x.Name})
            };

        }
    }
}
