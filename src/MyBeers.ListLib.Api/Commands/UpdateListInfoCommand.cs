using MyBeers.Common.CommonInterfaces;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;

namespace MyBeers.ListLib.Api.Commands
{
    public class UpdateListInfoCommand : ICommand
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<string> CollaboratorIds { get; set; }
    }
}
