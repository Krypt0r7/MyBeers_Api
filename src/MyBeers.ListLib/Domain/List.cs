using MyBeers.Common.MongoSettings;
using System;
using System.Collections.Generic;

namespace MyBeers.ListLib.Domain
{
    [BsonCollectioin("List")]
    public class List : MongoEntity
    {
        public List<string> BeerIds { get; set; } = new List<string>();
        public string OwnerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Collaborators { get; set; } = new List<string>();
    }
}
