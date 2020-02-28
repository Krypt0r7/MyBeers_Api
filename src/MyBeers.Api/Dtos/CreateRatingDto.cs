using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeers.Api.Dtos
{
    public class CreateRatingDto
    {
        public int Rating { get; set; }
        public string Description { get; set; }
        public string BeerId { get; set; }
    }
}
