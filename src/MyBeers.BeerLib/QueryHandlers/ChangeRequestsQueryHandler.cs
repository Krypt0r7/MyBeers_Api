using MyBeers.BeerLib.Api.Queries;
using MyBeers.BeerLib.Domain;
using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using MyBeers.UserLib.Api.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBeers.BeerLib.QueryHandlers
{
    public class ChangeRequestsQueryHandler : BaseQueryHandler<BeerChangeRequest, ChangeRequestsQuery, IEnumerable<ChangeRequestsQuery.ChangeRequest>>
    {
        public ChangeRequestsQueryHandler(IMongoRepository<BeerChangeRequest> repository, IQueryDispatcher queryDispatcher) : base(repository, queryDispatcher)
        {
        }

        public async override Task<IEnumerable<ChangeRequestsQuery.ChangeRequest>> HandleAsync(ChangeRequestsQuery query)
        {
            var changeRequests =  Repository.AsQueryable().OrderBy(x => x.Status).ThenBy(x => x.DateCreated).ToList();

            return changeRequests.Select(x => new ChangeRequestsQuery.ChangeRequest
            {
                Id = x.Id.ToString(),
                Status = x.Status.ToString(),
                DateCreated = x.DateCreated,
                User = GetUser(x.UserId),
                NewBeer = new Api.Queries.Models.Beer
                {
                    AlcoholPercentage = x.NewBeerInfo.AlcoholPercentage,
                    City = x.NewBeerInfo.City,
                    Country = x.NewBeerInfo.Country,
                    Id = x.NewBeerInfo.Id,
                    ImageUrl = x.NewBeerInfo.ImageUrl,
                    Name = x.NewBeerInfo.Name,
                    Producer = x.NewBeerInfo.Producer,
                    State = x.NewBeerInfo.State,
                    Style = x.NewBeerInfo.Style,
                    Type = x.NewBeerInfo.Type,
                    Containers = x.NewBeerInfo.Containers.Select(c => new Api.Queries.Models.Container
                    {
                        Id = c.Id,
                        Type = c.Type,
                        Price = c.Price,
                        ProductIdFromSystemet = c.ProductIdFromSystmet,
                        RecycleFee = c.RecycleFee,
                        SellStartDate = c.SellStartDate,
                        Volume = c.Volume,
                        Ypk = c.Ypk
                    })
                },
                OldBeer = new Api.Queries.Models.Beer
                {
                    AlcoholPercentage = x.NewBeerInfo.AlcoholPercentage,
                    City = x.NewBeerInfo.City,
                    Country = x.NewBeerInfo.Country,
                    Id = x.NewBeerInfo.Id,
                    ImageUrl = x.NewBeerInfo.ImageUrl,
                    Name = x.NewBeerInfo.Name,
                    Producer = x.NewBeerInfo.Producer,
                    State = x.NewBeerInfo.State,
                    Style = x.NewBeerInfo.Style,
                    Type = x.NewBeerInfo.Type,
                    Containers = x.NewBeerInfo.Containers.Select(c => new Api.Queries.Models.Container
                    {
                        Id = c.Id,
                        Type = c.Type,
                        Price = c.Price,
                        ProductIdFromSystemet = c.ProductIdFromSystmet,
                        RecycleFee = c.RecycleFee,
                        SellStartDate = c.SellStartDate,
                        Volume = c.Volume,
                        Ypk = c.Ypk
                    })
                }
            });
        }

        private ChangeRequestsQuery.User GetUser(string userId)
        {
            var user = QueryDispatcher.DispatchAsync<UserQuery, UserQuery.User>(new UserQuery { Id = userId }).Result;
            return new ChangeRequestsQuery.User 
            { 
                Id = user.Id.ToString(),
                Username = user.Username, 
                AvatarUrl = user.AvatarUrl,
                Email = user.Email
            };

        }
    }
}
