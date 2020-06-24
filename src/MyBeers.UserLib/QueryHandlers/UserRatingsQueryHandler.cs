using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using MyBeers.UserLib.Domain;
using MyBeers.UserLib.Api.Queries;
using System;
using System.Threading.Tasks;

namespace MyBeers.UserLib.QueryHandlers
{
    public class UserRatingsQueryHandler : BaseQueryHandler<Domain.User, UserRatingsQuery, UserRatingsQuery.UserRatings>
    {
        public UserRatingsQueryHandler(IMongoRepository<User> repository, IQueryDispatcher queryDispatcher) : base(repository, queryDispatcher)
        {
        }

        public override UserRatingsQuery.UserRatings Handle(UserRatingsQuery query)
        {
            throw new NotImplementedException();
        }
    }
}
