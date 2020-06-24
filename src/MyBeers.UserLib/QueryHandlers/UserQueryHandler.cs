using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using MyBeers.UserLib.Api.Queries;
using System;
using System.Threading.Tasks;

namespace MyBeers.UserLib.QueryHandlers
{
    public class UserQueryHandler : BaseQueryHandler<Domain.User,UserQuery, UserQuery.User>
    {
        public UserQueryHandler(IMongoRepository<Domain.User> repository, IQueryDispatcher queryDispatcher) : base(repository, queryDispatcher){}
        public override UserQuery.User Handle(UserQuery query)
        {
            var user = Repository.FindById(query.Id);

            if (user == null)
                throw new Exception("User not found");

            return new UserQuery.User
            {
                Id = user.Id.ToString(),
                AvatarUrl = user.AvatarUrl,
                Email = user.Email,
                Username = user.Username
            };
        }
    }
}
