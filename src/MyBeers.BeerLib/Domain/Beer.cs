using MyBeers.Common.MongoSettings;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBeers.BeerLib.Domain
{
    [BsonCollectioin("Beer")]
    public class Beer : MongoEntity
    {
        public string Name { get; set; }
        public string Producer { get; set; }
        public string Container { get; set; }
        public double RecycleFee { get; set; }
        public double AlcoholPercentage { get; set; }
        public double Volume { get; set; }
        public double Price { get; set; }
        public double YPK { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Type { get; set; }
        public string Style { get; set; }
        public string ProductionScale { get; set; }
        public string Usage { get; set; }
        public string Taste { get; set; }
        public DateTime SellStartDate { get; set; }
        public long ProductIdSystemet { get; set; }
        public string ImageUrl { get; set; }
    }
}
