using MyBeers.Common.CommonInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBeers.UserLib.Api.Commands
{
    public class UpdatePasswordCommand : ICommand
    {
        public string Id { get; }
        public string Password { get; }

        public UpdatePasswordCommand(string id, string password)
        {
            Id = id;
            Password = password;
        }
    }
}
