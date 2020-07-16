using MyBeers.Common.CommonInterfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MyBeers.UserLib.Api.Queries
{
    public class UsernamesQuery : IQuery<IEnumerable<UsernamesQuery.User>>
    {

        public class User
        {
            public string Id { get; set; }
            public string Username { get; set; }
        }
    }
}
