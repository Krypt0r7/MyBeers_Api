using MyBeers.Common.MongoSettings;
using System;
using System.Collections.Generic;

namespace MyBeers.BeerLib.Domain
{
    [BsonCollectioin("Beer")]
    public class OldBeer : MongoEntity
    {
        public string Name { get; set; }
        public string Producer { get; set; }
        public IEnumerable<OdlContainer> Containers { get; set; }
        public double AlcoholPercentage { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Type { get; set; }
        public string Style { get; set; }
        public string ImageUrl { get; set; }
        public MoreInformation MoreInformation { get; set; }

    }
}
