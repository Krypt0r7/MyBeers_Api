using System;
using System.Collections.Generic;
using System.Text;

namespace MyBeers.Common.MongoSettings
{
    public class BsonCollectioinAttribute : Attribute
    {
        public string CollectionName { get; }
        public BsonCollectioinAttribute(string collectionName)
        {
            CollectionName = collectionName;
        }
    }
}
