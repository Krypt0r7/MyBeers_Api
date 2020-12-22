using System;
using System.Collections.Generic;
using System.Text;

namespace MyBeers.BeerLib.Domain
{
    public class BeerRequestModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Producer { get; set; }
        public IEnumerable<ContainerModel> Containers { get; set; }
        public double AlcoholPercentage { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Type { get; set; }
        public string Style { get; set; }
        public string ImageUrl { get; set; }
    }

    public class ContainerModel
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public double Volume { get; set; }
        public double Price { get; set; }
        public double RecycleFee { get; set; }
        public double Ypk { get; set; }
        public DateTime SellStartDate { get; set; }
        public int ProductIdFromSystmet { get; set; }
    }
}
