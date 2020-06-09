using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyBeers.Common.CommonInterfaces
{
    public interface ICommandHandler { };
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command);
    }
}
