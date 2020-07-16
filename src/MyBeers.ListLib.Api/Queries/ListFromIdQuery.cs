using MyBeers.Common.CommonInterfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MyBeers.ListLib.Api.Queries
{
    public class ListFromIdQuery : IQuery<ListFromIdQuery.List>
    {
        public string Id { get; set; }

        public class List
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public User Owner { get; set; }
            public IEnumerable<Beer> Beers { get; set; }
            public IEnumerable<User> Collaborators { get; set; }
        }

        public class Beer
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Producer { get; set; }
            public double Price { get; set; }
            public double Alcohol { get; set; }
            public string Country { get; set; }
            public string State { get; set; }
            public string City { get; set; }
            public string ImageUrl { get; set; }
            public long SystemetProdId { get; set; }
        }

        public class User
        {
            public string Id { get; set; }
            public string Username { get; set; }
            public string AvatarUrl { get; set; }
        }
    }
}
