using MyBeers.Common.CommonInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBeers.ListLib.Api.Commands
{
    public class DeleteListCommand : ICommand
    {
        public string Id { get; set; }
    }
}
