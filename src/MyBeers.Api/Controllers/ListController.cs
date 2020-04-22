using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MyBeers.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBeers.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ListController : Controller
    {
        private readonly IListService _listservice;
        public ListController(IListService listservice)
        {
            _listservice = listservice;
        }


    }
}
