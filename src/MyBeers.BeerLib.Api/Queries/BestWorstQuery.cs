using MyBeers.Common.CommonInterfaces;
using System.Collections.Generic;

namespace MyBeers.BeerLib.Api.Queries
{
    public class BestWorstQuery : IQuery<BestWorstQuery.BestWorst>
    {
        public class BestWorst
        {
            public IEnumerable<Beer> BestBeer { get; set; }
            public IEnumerable<Beer> WorstBeer { get; set; }

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
                public double AvarageRating { get; set; }
                public string ImageUrl { get; set; }
            }
        }

    }
}
