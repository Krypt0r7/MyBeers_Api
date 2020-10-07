using System;

namespace MyBeers.BeerLib.Domain
{
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
