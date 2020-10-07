using System;
using System.Collections.Generic;
using MyBeers.Common.MongoSettings;

namespace MyBeers.BeerLib.Domain
{
    [BsonCollectioin("ChangeRequest")]
    public class BeerChangeRequest : MongoEntity
    {
        public string UserId { get; set; }
        public string BeerId { get; set; }
        public Status ChangeStatus { get; set; }
        public DateTime DateCreated { get; set; }
        public IEnumerable<Change> Changes { get; set; }
        public class Change 
        {
            public string Property { get; set; }
            public object OldValue { get; set; }
            public object NewValue { get; set; }
        }
        public enum Status
        {
            Completed,
            New,
            Declined
        }
    }
}