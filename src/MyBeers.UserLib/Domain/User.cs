using MyBeers.Common.MongoSettings;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBeers.UserLib.Domain
{
    [BsonCollectioin("User")]
    public class User : MongoEntity
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
