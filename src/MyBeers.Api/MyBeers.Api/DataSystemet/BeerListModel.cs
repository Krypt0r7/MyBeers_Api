using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeers.Api.DataSystemet
{
    public class BeerListModel
    {
        public IList<SystemetBeerIn> Hits { get; set; }
    }
}
