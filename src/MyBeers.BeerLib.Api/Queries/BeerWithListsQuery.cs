﻿using MyBeers.Common.CommonInterfaces;
using System.Collections.Generic;

namespace MyBeers.BeerLib.Api.Queries
{
    public class BeerWithListsQuery : IQuery<BeerWithListsQuery.Beer>
    {
        public string Id { get; set; }
        public string UserId { get; set; }

        public class Beer
        {
            public string Id { get; set; }
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
            public long ProductIdSystemet { get; set; }
            public string ImageUrl { get; set; }
            public IEnumerable<List> Lists { get; set; }
        }

        public class List
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }
    }
}
