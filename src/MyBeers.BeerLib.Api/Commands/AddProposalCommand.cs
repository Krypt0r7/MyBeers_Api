using System;
using System.Collections.Generic;
using MyBeers.Common.CommonInterfaces;

namespace MyBeers.BeerLib.Api.Commands {

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
        public List<Container> Containers { get; set; }
        public double AlcoholPercentage { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Type { get; set; }
        public string Style { get; set; }
        public SystemetInformation MoreInformationModel { get; set; }
        public string Image { get; set; }

        public class Container
        {
            public string Type { get; set; }
            public double Volume { get; set; }
            public double Price { get; set; }
            public double RecycleFee { get; set; }
            public DateTime SellStartDate { get; set; }
            public string ProductionScale { get; set; }
            public double Ypk { get; set; }
            public int ProductIdFromSystemet { get; set; }
        }

        public class SystemetInformation 
        {
            public int ProductId { get; set; }
            public int ProductNumber { get; set; }
            public string ProductNameBold { get; set; }
            public string ProductNameThin { get; set; }
            public string Category { get; set; }
            public int ProductNumberShort { get; set; }
            public string ProducerName { get; set; }
            public string SupplierName { get; set; }
            public string BottleTextShort { get; set; }
            public string Seal { get; set; }
            public bool IsOrganic { get; set; }
            public DateTime SellStartDate { get; set; }
            public double AlcoholPercentage { get; set; }
            public double Volume { get; set; }
            public double Price { get; set; }
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
            public double RecycleFee { get; set; }
            public bool IsNews { get; set; }
        }
    }
}