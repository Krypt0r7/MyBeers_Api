using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBeers.Migration.DataModels
{
    public class Beer
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime Added { get; set; }
        public double YPK { get; set; }
        public BeerDataModel BeerData { get; set; }

        public class BeerDataModel
        {
            public long ProductId { get; set; }
            public long ProductNumber { get; set; }
            public string ProductName { get; set; }
            public string Category { get; set; }
            public int ProductNumberShort { get; set; }
            public string ProducerName { get; set; }
            public string SupplierName { get; set; }
            public string BottleTextShort { get; set; }
            public DateTime SellStartDate { get; set; }
            public double AlcoholPercentage { get; set; }
            public double Volume { get; set; }
            public double Price { get; set; }
            public string Country { get; set; }
            public string OriginLevel1 { get; set; }
            public string OriginLevel2 { get; set; }
            public string SubCategory { get; set; }
            public string Type { get; set; }
            public string Style { get; set; }
            public string AssortmentText { get; set; }
            public string BeverageDescriptionShort { get; set; }
            public string Usage { get; set; }
            public string Taste { get; set; }
            public string Assortment { get; set; }
            public double RecycleFee { get; set; }
            public string ImageUrl { get; set; }
        }
    }
}
