using MyBeers.Common.CommonInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBeers.RatingLib.Api.Commands
{
    public class CreateUpdateRatingCommand : ICommand
    {

        public int Taste { get; }
        public int AfterTaste { get; }
        public int Chugability { get; }
        public int Value { get; }
        public int FirstImpression { get; }
        public string Description { get; }
        public string BeerId { get; }
        public string UserId { get; }

        public CreateUpdateRatingCommand(int taste, int afterTaste, int chugability, int value, int firstImpression, string description, string beerId, string userId)
        {
            Taste = taste;
            AfterTaste = afterTaste;
            Chugability = chugability;
            Value = value;
            FirstImpression = firstImpression;
            Description = description;
            BeerId = beerId;
            UserId = userId;
        }
        
    }
}
