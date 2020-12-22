using MongoDB.Bson;
using MyBeers.Common.MongoSettings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyBeers.BeerLib.Domain
{
    [BsonCollectioin("Mongo_Beer")]
    public class Beer : MongoEntity
    {
        public string Name { get; private set; }
        public string Producer { get; set; }
        public List<Container> Containers { get; private set; } = new List<Container>();
        public double AlcoholPercentage { get; private set; }
        public string Country { get; private set; }
        public string State { get; private set; }
        public string City { get; private set; }
        public string Type { get; private set; }
        public string Style { get; private set; }
        public List<string> Hops { get; private set; } = new List<string>();
        public bool InProduction { get; private set; }
        public string ImageUrl { get; private set; }

        public Beer(string id, string name, string producer, double alcoholPrecentage, string country, string state, string city, string type, string style, string imageUrl, bool inProduction, List<string> hops, List<Container> containers)
        {
            Id = new ObjectId(id);
            Name = name;
            Producer = producer;
            Containers = containers;
            AlcoholPercentage = alcoholPrecentage;
            Country = country;
            State = state;
            City = city;
            Type = type;
            Hops = hops;
            InProduction = inProduction;
            Style = style;
            ImageUrl = imageUrl;
        }

        public void AddContainer(string id, ContainerType type, double volume, double price, double recycleFee, double ypk, DateTime sellStartDate, int productIdFromSystemet)
        {
            Containers.Add(new Container(id, type, volume, price, recycleFee, ypk, sellStartDate, productIdFromSystemet));
        }

        public void RemoveContainer(string id)
        {
            Containers.RemoveAll(x => x.Id == id);
        }

        public void AddHops(string hops)
        {
            var exists = Hops.FirstOrDefault(x => x.Equals(hops));
            if (exists != null)
            {
                Hops.Add(hops);
            }
            
        }

        public void RemoveHops(string hops)
        {

        }
    }
}
