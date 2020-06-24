using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using MyBeers.UserLib.Api.Queries;
using MyBeers.UserLib.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyBeers.UserLib.QueryHandlers
{
    public class UserByOldIdQueryHandler : BaseQueryHandler<Domain.User, UserByOldIdQuery, UserByOldIdQuery.User>
    {
        public UserByOldIdQueryHandler(IMongoRepository<User> repository, IQueryDispatcher queryDispatcher) : base(repository, queryDispatcher)
        {
        }

        public override UserByOldIdQuery.User Handle(UserByOldIdQuery query)
        {
            var user = Repository.FindOne(filter => filter.OldId == query.OldId);
            return new UserByOldIdQuery.User
            {
                AvatarUrl = user.AvatarUrl,
                Email = user.Email,
                Id = user.Id.ToString(),
                Username = user.Username
            };
        }
    }
}
