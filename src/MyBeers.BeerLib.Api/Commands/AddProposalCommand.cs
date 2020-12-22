using System;
using System.Collections.Generic;
using MyBeers.Common.CommonInterfaces;

namespace MyBeers.BeerLib.Api.Commands 
{

    public class AddProposalCommand : ICommand
    {
        public string BeerId { get; set; }
        public BeerData BeerData { get; set; }
    }

    public class BeerData 
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Producer { get; set; }
        public double AlcoholPercentage { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Type { get; set; }
        public string Style { get; set; }
        public string Image { get; set; }
        public List<Container> Containers { get; set; }

        public class Container
        {
            public string Id { get; set; }
            public string Type { get; set; }
            public double Volume { get; set; }
            public double Price { get; set; }
            public double RecycleFee { get; set; }
            public DateTime SellStartDate { get; set; }
            public double Ypk { get; set; }
            public int ProductIdFromSystemet { get; set; }
        }
    }
}