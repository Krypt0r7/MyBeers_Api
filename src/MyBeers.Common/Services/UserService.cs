using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyBeers.Common.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public UserService(IHttpContextAccessor contextAccessor)
        {
            this._contextAccessor = contextAccessor;
        }
        public string GetUserId()
        {
            return _contextAccessor?.HttpContext?.User.Identity.Name;
        }
    }

    public interface IUserService
    {
        string GetUserId();
    }
}
