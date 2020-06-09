using MyBeers.Common.CommonInterfaces;
using System.Collections.Generic;

namespace MyBeers.UserLib.Api.Queries
{
    public class UserRatingsQuery : IQuery<UserRatingsQuery.UserRatings>
    {
        public string Id { get; set; }

        public class UserRatings
        {
            public string Id { get; set; }
            public string Username { get; set; }
            public string AvatarUrl { get; set; }
            public IEnumerable<Rating> Ratings { get; set; }

            public class Rating
            {
                public double OverallRating { get; set; }
                public int Taste { get; set; }
                public int AfterTaste { get; set; }
                public int Chugability { get; set; }
                public int Value { get; set; }
                public int FirstImpression { get; set; }
                public string Description { get; set; }
                public Beer Beer { get; set; }
            }
            public class Beer
            {
                public string Name { get; set; }
                public string Producer { get; set; }
            }

        }
    }
}
