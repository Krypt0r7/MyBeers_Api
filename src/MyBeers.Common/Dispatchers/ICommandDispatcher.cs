using MyBeers.Common.CommonInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyBeers.Common.Dispatchers
{
    public interface ICommandDispatcher
    {
        Task DispatchAsync<T>(T command) where T : ICommand;
    }
}
