using System;
using System.Collections.Generic;
using System.Text;

namespace MyBeers.BeerLib.Api.Queries.Models
{
    public class Container
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public double Volume { get; set; }
        public double Price { get; set; }
        public double RecycleFee { get; set; }
        public double Ypk { get; set; }
        public DateTime SellStartDate { get; set; }
        public int ProductIdFromSystemet { get; set; }
    }
}
