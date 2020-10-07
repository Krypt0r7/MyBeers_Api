using MyBeers.Common.CommonInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBeers.RatingLib.Api.Commands
{
    public class CreateUpdateRatingCommand : ICommand
    {
        public int Taste { get; set;  }
        public int AfterTaste { get; set; }
        public int Chugability { get; set; }
        public int Value { get; set; }
        public int FirstImpression { get; set; }
        public string Description { get; set; }
        public string BeerId { get; set; }
    }
}
