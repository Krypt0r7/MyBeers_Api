using MyBeers.Common.CommonInterfaces;

namespace MyBeers.UserLib.Api.Commands
{
    public class CreateUserCommand : ICommand
    {
        public string Username { get; set; }
        public string Password { get; set;  }
        public string Email { get; set;  }
        public string OldId { get; set; }
        public string Role { get; set; }
    }
}
