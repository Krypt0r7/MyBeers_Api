using MyBeers.Common.CommonInterfaces;

namespace MyBeers.ListLib.Api.Commands
{
    public class UpdateListCommand : ICommand
    {
        public string ListId { get; set; }
        public string BeerId { get; set; }
        public UpdateListCommand(string listId, string beerId)
        {
            ListId = listId;
            BeerId = beerId;
        }
    }
}
