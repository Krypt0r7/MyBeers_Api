using MyBeers.Common.MongoSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeers.RatingLib.Domain
{
    [BsonCollectioin("Rating")]
    public class Rating : MongoEntity
    {
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
