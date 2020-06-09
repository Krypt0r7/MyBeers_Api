using MyBeers.Common.CommonInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyBeers.Common.Dispatchers
{
    public interface IQueryDispatcher
    {
        Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>;
    }
}
