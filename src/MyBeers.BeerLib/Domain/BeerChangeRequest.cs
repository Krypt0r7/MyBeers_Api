using System;
using System.Collections.Generic;
using MyBeers.Common.MongoSettings;

namespace MyBeers.BeerLib.Domain
{
    [BsonCollectioin("ChangeRequest")]
    public class BeerChangeRequest : MongoEntity
    {
        public string UserId { get; set; }
        public Status Status { get; set; }
        public DateTime DateCreated { get; set; }
        public BeerRequestModel OldBeerInfo { get; set; }
        public BeerRequestModel NewBeerInfo { get; set; }
    }
    public enum Status
    {
        Completed,
        New,
        Declined
    }
}