using MyBeers.Common.CommonInterfaces;
using System.Collections.Generic;

namespace MyBeers.BeerLib.Api.Queries
{
    public class SearchBeerQuery : IQuery<IEnumerable<SearchBeerQuery.Beer>>
    {
        public string SearchString { get; set; }

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
