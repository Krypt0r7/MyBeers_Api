using MyBeers.Common.CommonInterfaces;
using System.Collections.Generic;

namespace MyBeers.ListLib.Api.Queries
{
    public class ListsByUserIdQuery : IQuery<IEnumerable<ListsByUserIdQuery.ListByUserId>>
    {
        public string UserId { get; set; }
        public class ListByUserId
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public User Owner { get; set; }
            public IEnumerable<Beer> Beers { get; set; }
        }

        public class User
        {
            public string UserName { get; set; }
            public string AvatarUrl { get; set; }
        }

        public class Beer
        {
            public string Id { get; set; }
            public string ProductName { get; set; }
            public string Name { get; set; }
        }
    }
}
