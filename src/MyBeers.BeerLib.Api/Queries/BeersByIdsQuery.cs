using MyBeers.Common.CommonInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBeers.BeerLib.Api.Queries
{
    public class BeersByIdsQuery : IQuery<IEnumerable<BeersByIdsQuery.Beer>>
    {
        public List<string> BeerIds { get; set; }

        public class Beer
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Producer { get; set; }
            public double Price { get; set; }
            public double Alcohol { get; set; }
            public string Country { get; set; }
            public string State { get; set; }
            public string City { get; set; }
            public string ImageUrl { get; set; }
            public long SystemetProdId { get; set; }
        }
    }
}
