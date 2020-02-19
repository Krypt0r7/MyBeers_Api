using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeers.Api.Utils
{
    public interface IMongoSettings
    {
        string BeerCollection { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string UserCollection { get; set; }
        string RatingCollection { get; set; }
    }

    public class MongoSettings : IMongoSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string BeerCollection { get; set; }
        public string UserCollection { get; set; }
        public string RatingCollection { get; set; }
    }
}
