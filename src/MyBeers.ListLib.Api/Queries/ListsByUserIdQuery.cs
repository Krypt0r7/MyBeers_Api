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
        }

    }
}
