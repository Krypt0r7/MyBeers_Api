using MyBeers.Common.CommonInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBeers.UserLib.Api.Commands
{
    public class DeleteUserCommand : ICommand
    {
        public string Id { get; set; }
    }
}
