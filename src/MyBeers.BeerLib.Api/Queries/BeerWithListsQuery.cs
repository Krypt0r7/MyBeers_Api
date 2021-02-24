using MyBeers.Common.CommonInterfaces;
using System.Collections.Generic;


namespace MyBeers.BeerLib.Api.Queries
{
    public class BeerWithListsQuery : IQuery<BeerWithListsQuery.Beer>
    {
        public string Id { get; set; }

        public class Beer : Models.Beer
        {

            public IEnumerable<List> Lists { get; set; }
        }

        public class List
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }
    }
}
