using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeers.Common.MongoSettings
{
    public interface IDBSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }

    public class DBSettings : IDBSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
