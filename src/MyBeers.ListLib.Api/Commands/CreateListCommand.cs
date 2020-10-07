using MyBeers.Common.CommonInterfaces;

namespace MyBeers.ListLib.Api.Commands
{
    public class CreateListCommand : ICommand
    {
        public string Name { get; set;  }
        public string Description { get; set;  }
     
    }
}
