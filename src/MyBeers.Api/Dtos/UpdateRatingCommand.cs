using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeers.Api.Dtos
{
    public class UpdateRatingCommand
    {
        public int Rating { get; set; }
        public string Description { get; set; }
    }
}
