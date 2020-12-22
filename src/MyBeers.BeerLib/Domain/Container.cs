using System;

namespace MyBeers.BeerLib.Domain
{
    public class Container
    {
        public string Id { get; set; }
        public ContainerType Type { get; set; }
        public double Volume { get; set; }
        public double Price { get; set; }
        public double RecycleFee { get; set; }
        public double Ypk { get; set; }
        public DateTime SellStartDate { get; set; }
        public int ProductIdFromSystmet { get; set; }

        public Container(string id, ContainerType type, double volume, double price, double recycleFee, double ypk, DateTime sellStartDate, int productIdFromSystemet)
        {
            Id = id;
            Type = type;
            Volume = volume;
            Price = price;
            RecycleFee = recycleFee;
            Ypk = ypk;
            SellStartDate = sellStartDate;
            ProductIdFromSystmet = productIdFromSystemet;
        }
    }
}