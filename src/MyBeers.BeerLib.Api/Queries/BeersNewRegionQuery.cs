using MyBeers.Common.CommonInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBeers.BeerLib.Api.Queries
{
    public class BeersNewRegionQuery : IQuery<IEnumerable<BeersNewRegionQuery.Beer>>
    {
        public string Region { get; set; }

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
            public string Type { get; set; }
            public string ImageUrl { get; set; }
        }
    }
}
