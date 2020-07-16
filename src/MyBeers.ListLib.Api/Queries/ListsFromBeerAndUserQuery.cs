using MyBeers.Common.CommonInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBeers.ListLib.Api.Queries
{
    public class ListsFromBeerAndUserQuery : IQuery<IEnumerable<ListsFromBeerAndUserQuery.List>>
    {
        public string UserId { get; set; }
        public string BeerId { get; set; }

        public class List
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public DateTime Created { get; set; }
        }
    }
}
