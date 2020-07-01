using MyBeers.Common.CommonInterfaces;
using System.Collections;
using System.Collections.Generic;

namespace MyBeers.ListLib.Api.Commands
{
    public class UpdateListCommand : ICommand
    {
        public string ListId { get; set; }
        public IEnumerable<string> BeerIds { get; set; }
    }
}
