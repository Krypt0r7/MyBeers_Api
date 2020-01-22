using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyBeers.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class BeerController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> BeersAsync()
        {
            return Ok();
        }
    }
}