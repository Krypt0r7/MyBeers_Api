using System.Collections.Generic;

namespace MyBeers.BeerLib.Api.Queries.Models
{
    public class Beer
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Producer { get; set; }
        public IEnumerable<Container> Containers { get; set; }
        public double AlcoholPercentage { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Type { get; set; }
        public string Style { get; set; }
        public string ImageUrl { get; set; }
    }
}
