using MyBeers.Common.CommonInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBeers.ListLib.Api.Commands
{
    public class UpdateCollaboratorsCommand : ICommand
    {
        public string Id { get; set; }
        public IEnumerable<string> UserIds { get; set; }
    }
}
