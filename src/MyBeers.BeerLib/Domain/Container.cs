using System;

namespace MyBeers.BeerLib.Domain
{
    public class Container
    {
        public string Type { get; set; }
        public double Volume { get; set; }
        public double Price { get; set; }
        public double RecycleFee { get; set; }
        public string ProductionScale { get; set; }
        public double Ypk { get; set; }
        public DateTime SellStartDate { get; set; }
        public int ProductIdFromSystmet { get; set; }
    }
}