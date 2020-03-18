using MyBeers.Api.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeers.Api.Dtos
{
    public class BeerAverageRatingDto
    {
        public string Id { get; set; }
        public DateTime Added { get; set; }
        public double YPK { get; set; }
        public BeerData BeerData { get; set; }
        public double AverageRating { get; set; }
    }
}
