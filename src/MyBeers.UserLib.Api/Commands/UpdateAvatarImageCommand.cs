using MyBeers.Common.CommonInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBeers.UserLib.Api.Commands
{
    public class UpdateAvatarImageCommand : ICommand
    {
        public string Id { get; }
        public string ImageData { get; }
        public UpdateAvatarImageCommand(string id, string imageData)
        {
            Id = id;
            ImageData = imageData;
        }
    }
}
