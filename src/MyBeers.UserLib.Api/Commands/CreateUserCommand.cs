using MyBeers.Common.CommonInterfaces;

namespace MyBeers.UserLib.Api.Commands
{
    public class CreateUserCommand : ICommand
    {
        public string Username { get; }
        public string Password { get; }
        public string Email { get; }
        public string OldId { get; }

        public CreateUserCommand(string username, string password, string email, string oldId)
        {
            Username = username;
            Password = password;
            Email = email;
            OldId = oldId;
        }
    }
}
