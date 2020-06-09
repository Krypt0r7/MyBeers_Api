using MyBeers.Common.CommonInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyBeers.Common.Dispatchers
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private IServiceProvider _serviceProvider;
        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }
        public async Task DispatchAsync<T>(T command) where T : ICommand
        {
            var service = this._serviceProvider.GetService(typeof(ICommandHandler<T>)) as ICommandHandler<T>;
            await service.HandleAsync(command);
        }
    }
}
