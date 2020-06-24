using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using MyBeers.UserLib.Domain;
using MyBeers.UserLib.Api.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeers.UserLib.QueryHandlers
{
    public class UsersQueryHandler : BaseQueryHandler<User, UsersQuery, IEnumerable<UsersQuery.User>>
    {
        public UsersQueryHandler(IMongoRepository<User> repository, IQueryDispatcher queryDispatcher) : base(repository, queryDispatcher){}

        public override IEnumerable<UsersQuery.User> Handle(UsersQuery query)
        {
            var users = Repository.FilterBy(filter => true);
            return users.Select(s => new UsersQuery.User { AvatarUrl = s.AvatarUrl, Email = s.Email, Id = s.Id.ToString(), Username = s.Username });
        }

    }
}
