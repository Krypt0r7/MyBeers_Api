using MyBeers.Common.CommonInterfaces;

namespace MyBeers.UserLib.Api.Commands
{
    public class CreateUserCommand : ICommand
    {
        public string Username { get; }
        public string Password { get; }
        public string Email { get; }

        public CreateUserCommand(string username, string password, string email)
        {
            Username = username;
            Password = password;
            Email = email;
        }
    }
}
