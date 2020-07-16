using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using MyBeers.ListLib.Api.Queries;
using MyBeers.ListLib.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBeers.ListLib.QueryHandlers
{
    public class ListsFromBeerAndUserQueryHandler : BaseQueryHandler<Domain.List, ListsFromBeerAndUserQuery, IEnumerable<ListsFromBeerAndUserQuery.List>>
    {
        public ListsFromBeerAndUserQueryHandler(IMongoRepository<List> repository, IQueryDispatcher queryDispatcher) : base(repository, queryDispatcher)
        {
        }

        public override async Task<IEnumerable<ListsFromBeerAndUserQuery.List>> HandleAsync(ListsFromBeerAndUserQuery query)
        {
            var lists = await Repository.FilterByAsync(filter => filter.OwnerId == query.UserId);

            return lists.Where(list => list.BeerIds.Contains(query.BeerId)).Select(list => new ListsFromBeerAndUserQuery.List
            {
                Description = list.Description,
                Id = list.Id.ToString(),
                Name = list.Name,
                Created = list.Created
            });
        }
    }
}
