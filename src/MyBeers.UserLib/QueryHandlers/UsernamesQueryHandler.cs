using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using MyBeers.UserLib.Api.Queries;
using MyBeers.UserLib.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBeers.UserLib.QueryHandlers
{
    public class UsernamesQueryHandler : BaseQueryHandler<Domain.User, UsernamesQuery, IEnumerable<UsernamesQuery.User>>
    {
        public UsernamesQueryHandler(IMongoRepository<User> repository, IQueryDispatcher queryDispatcher) : base(repository, queryDispatcher)
        {
        }

        public override async Task<IEnumerable<UsernamesQuery.User>> HandleAsync(UsernamesQuery query)
        {
            var users = await Repository.FilterByAsync(filter => true);

            return users.Select(user => new UsernamesQuery.User
            {
                Id = user.Id.ToString(),
                Username = user.Username
            });
        }
    }
}
