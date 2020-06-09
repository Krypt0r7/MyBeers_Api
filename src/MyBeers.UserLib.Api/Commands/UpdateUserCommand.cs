using MyBeers.Common.CommonInterfaces;

namespace MyBeers.UserLib.Api.Commands
{
    public class UpdateUserCommand : ICommand
    {
        public string Id { get; }
        public string Username { get; }
        public string Email { get; }

        public UpdateUserCommand(string id, string username, string email)
        {
            Id = id;
            Username = username;
            Email = email;
        }
    }
}
