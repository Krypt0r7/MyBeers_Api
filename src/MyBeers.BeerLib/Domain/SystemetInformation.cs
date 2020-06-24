using System;
using System.Collections.Generic;
using System.Text;

namespace MyBeers.BeerLib.Domain
{
    public class SystemetInformation
    {
        public long ProductId { get; set; }
        public long ProductNumber { get; set; }
        public string ProductNameBold { get; set; }
        public string ProductNameThin { get; set; }
        public string Category { get; set; }
        public long ProductNumberShort { get; set; }
        public string ProducerName { get; set; }
        public string SupplierName { get; set; }
        public bool IsKosher { get; set; }
        public string BottleTextShort { get; set; }
        public string Seal { get; set; }
        public long RestrictedParcelQuantity { get; set; }
        public bool IsOrganic { get; set; }
        public bool IsEthical { get; set; }
        public string EthicalLabel { get; set; }
        public bool IsWebLaunch { get; set; }
        public DateTime SellStartDate { get; set; }
        public bool IsCompletelyOutOfStock { get; set; }
        public bool IsTemporaryOutOfStock { get; set; }
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
        public bool IsManufacturingCountry { get; set; }
        public bool IsRegionalRestricted { get; set; }
        public bool IsNews { get; set; }
    }
}
