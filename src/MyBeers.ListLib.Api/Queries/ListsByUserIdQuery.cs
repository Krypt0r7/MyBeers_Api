using MyBeers.Common.CommonInterfaces;
using System.Collections.Generic;

namespace MyBeers.ListLib.Api.Queries
{
    public class ListsByUserIdQuery : IQuery<ListsByUserIdQuery.Lists>
    {
        public class Lists
        {
            public IEnumerable<List> MyLists { get; set; }
            public IEnumerable<List> CollaborateLists { get; set; }

        }
        public class List
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }

    }
}
