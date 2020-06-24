using MyBeers.Common.CommonInterfaces;

namespace MyBeers.Migration.Api.Commands
{
    public class MigrateDataCommand : ICommand
    {
        public bool Users { get; set; }
        public bool Ratings { get; set; }
    }
}
