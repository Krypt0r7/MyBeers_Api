using MyBeers.Api.Data;
using System;
using System.Collections.Generic;

namespace MyBeers.Api.Dtos
{
    public class BeersAndRatingsQueryDto
    {
        public string Id { get; set; }
        public DateTime Added { get; set; }
        public double YPK { get; set; }
        public BeerData BeerData { get; set; }
        public List<RatingAndUsersQueryDto> Ratings { get; set; }
    }
}
