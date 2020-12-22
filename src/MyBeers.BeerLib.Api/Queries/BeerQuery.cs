using System;
using System.Collections.Generic;
using MyBeers.Common.CommonInterfaces;

namespace MyBeers.BeerLib.Api.Queries
{
    public class BeerQuery : IQuery<BeerQuery.Beer>
    {
        public string Id { get; set; }

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
    }
    
}
