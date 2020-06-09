﻿using MyBeers.BeerLib.Api.Queries;
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
    public class ListsByUserIdQueryHandler : BaseQueryHandler<Domain.List, ListsByUserIdQuery, IEnumerable<ListsByUserIdQuery.ListByUserId>>
    {
        public ListsByUserIdQueryHandler(IMongoRepository<List> repository, IQueryDispatcher queryDispatcher) : base(repository, queryDispatcher)
        {
        }

        public override async Task<IEnumerable<ListsByUserIdQuery.ListByUserId>> HandleAsync(ListsByUserIdQuery query)
        {
            var lists = Repository.FilterBy(filter => filter.OwnerId == query.UserId).ToList();

            var user = await this.QueryDispatcher.DispatchAsync<UserQuery, UserQuery.User>(
                new UserQuery
                {
                    Id = query.UserId
                });

            return lists.Select(x => new ListsByUserIdQuery.ListByUserId
            {
                Id = x.Id.ToString(),
                Description = x.Description,
                Name = x.Name,
                Owner = new ListsByUserIdQuery.User
                {
                    AvatarUrl = user.AvatarUrl,
                    UserName = user.Username
                },
                Beers = GetBeers(x.BeerIds)
            });

        }

        private IEnumerable<ListsByUserIdQuery.Beer> GetBeers(List<string> beerIds)
        {
            var beers = QueryDispatcher.DispatchAsync<BeersByIdsQuery, IEnumerable<BeersByIdsQuery.Beer>>(new BeersByIdsQuery { BeerIds = beerIds }).Result;
           
            return beers.Select(beer => new ListsByUserIdQuery.Beer { ProductName = beer.Producer, Id = beer.Id, Name = beer.Name });
        }
    }
}