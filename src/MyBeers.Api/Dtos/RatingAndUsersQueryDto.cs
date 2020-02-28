using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeers.Api.Dtos
{
    public class RatingAndUsersQueryDto
    {
        public string Id { get; set; }
        public int OverallRating { get; set; }
        public string Description { get; set; }
        public DateTime CreatedTime { get; set; }
        public UserDto User { get; set; }
    }
}
