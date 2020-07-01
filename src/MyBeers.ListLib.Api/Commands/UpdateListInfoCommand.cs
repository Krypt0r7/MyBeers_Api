using MyBeers.Common.CommonInterfaces;

namespace MyBeers.ListLib.Api.Commands
{
    public class UpdateListInfoCommand : ICommand
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
