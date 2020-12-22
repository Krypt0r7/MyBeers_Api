using MyBeers.Common.MongoSettings;

namespace MyBeers.UserLib.Domain
{
    [BsonCollectioin("User")]
    public class User : MongoEntity
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string AvatarUrl { get; set; }
        public string OldId { get; set; }
        public string Role { get; set; } = UserLib.Role.User;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
