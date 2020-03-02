using MyBeers.Api.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeers.Api.Dtos
{
    public class CreateRatingCommand
    {
        public int OverallRating { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public Beer Beer { get; set; }
    }
}
