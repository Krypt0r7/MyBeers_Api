using MyBeers.Common.CommonInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBeers.RatingLib.Api.Queries
{
    public class RatingsByUserQuery : IQuery<IEnumerable<RatingsByUserQuery.Rating>>
    {
        public class Rating
        {
            public string Id { get; set; }
            public double OverallRating { get; set; }
            public int Taste { get; set; }
            public int AfterTaste { get; set; }
            public int Chugability { get; set; }
            public int Value { get; set; }
            public int FirstImpression { get; set; }
            public string Description { get; set; }
            public string UserId { get; set; }
            public string BeerId { get; set; }
        }
    }
}
