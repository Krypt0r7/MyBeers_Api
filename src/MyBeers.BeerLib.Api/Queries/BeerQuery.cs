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
            public MoreInformation MoreInformationModel { get; set; }
            public string ImageUrl { get; set; }
            public class Container
            {
                public string Type { get; set; }
                public double Volume { get; set; }
                public double Price { get; set; }
                public double RecycleFee { get; set; }
                public string ProductionScale { get; set; }
                public double Ypk { get; set; }
                public DateTime SellStartDate { get; set; }
                public int ProductIdFromSystemet { get; set; }
            }
            public class MoreInformation
            {
                public int ProductId { get; set; }
                public int ProductNumber { get; set; }
                public string ProductNameBold { get; set; }
                public string ProductNameThin { get; set; }
                public string Category { get; set; }
                public int ProductNumberShort { get; set; }
                public string ProducerName { get; set; }
                public string SupplierName { get; set; }
                public bool IsOrganic { get; set; }
                public double AlcoholPercentage { get; set; }
                public string Country { get; set; }
                public string OriginLevel1 { get; set; }
                public string OriginLevel2 { get; set; }
                public int Vintage { get; set; }
                public string SubCategory { get; set; }
                public string Type { get; set; }
                public string Style { get; set; }
                public string AssortmentText { get; set; }
                public string BeverageDescriptionShort { get; set; }
                public string Usage { get; set; }
                public string Taste { get; set; }
                public string Assortment { get; set; }
                public bool IsNews { get; set; }
            }

        }
    }
    
}
