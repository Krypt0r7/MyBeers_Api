using MyBeers.Common.CommonInterfaces;

namespace MyBeers.ListLib.Api.Commands
{
    public class CreateListCommand : ICommand
    {
        public string UserId { get; }
        public string Name { get; }
        public string Description { get; }
        public CreateListCommand(string userId, string name, string description)
        {
            this.UserId = userId;
            this.Name = name;
            this.Description = description;
        }
    }
}
