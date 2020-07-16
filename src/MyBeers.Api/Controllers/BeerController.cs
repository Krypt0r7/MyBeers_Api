using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBeers.Api.Base;
using MyBeers.BeerLib.Api.Queries;
using MyBeers.BeerLib.Api.Commands;
using MyBeers.Common.Dispatchers;

namespace MyBeers.Api.Controllers
{
	[Authorize]
	[ApiController]
	[Route("[controller]/[action]")]
	public class BeerController : BaseController
	{
		
		public BeerController(IQueryDispatcher queryDispatcher,	ICommandDispatcher commandDispatcher) : base(queryDispatcher, commandDispatcher)
		{
		}

		[HttpPost("seed/create")]
		public async Task<IActionResult> CreateSeed(CreateBeerCommand createBeerCommand)
		{
			await CommandDispatcher.DispatchAsync(createBeerCommand);
			return Ok();
		}


		[HttpGet]
		public async Task<IActionResult> Beer([FromQuery]BeerQuery beerQuery)
		{
			try
			{
				var beer = await QueryDispatcher.DispatchAsync<BeerQuery, BeerQuery.Beer>(beerQuery);
				return Ok(beer);
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}

		}

		[HttpGet]
		public async Task<IActionResult> WithLists([FromQuery]BeerWithListsQuery beerWithListsQuery)
		{
			try
			{
				var beer = await QueryDispatcher.DispatchAsync<BeerWithListsQuery, BeerWithListsQuery.Beer>(beerWithListsQuery);
				return Ok(beer);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}



		[AllowAnonymous]
		[HttpGet]
		public async Task<IActionResult> BestWorst([FromQuery]BestWorstQuery bestWorstQuery)
		{
			try
			{
				var lists = await QueryDispatcher.DispatchAsync<BestWorstQuery, BestWorstQuery.BestWorst>(bestWorstQuery);

				return Ok(lists);
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}
		}

		[HttpGet]
		public async Task<IActionResult> Ratings([FromQuery]BeerRatingsQuery beerRatingsQuery)
		{
			try
			{
				var beer = await QueryDispatcher.DispatchAsync<BeerRatingsQuery, BeerRatingsQuery.Beer>(beerRatingsQuery);
				return Ok(beer);
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}
		}

		[AllowAnonymous]
		[HttpGet]
		public async Task<IActionResult> Search([FromQuery]SearchBeerQuery searchBeerQuery)
		{
			try
			{
				var result = await QueryDispatcher.DispatchAsync<SearchBeerQuery, IEnumerable<SearchBeerQuery.Beer>>(searchBeerQuery);
				return Ok(result);
			}
			catch (Exception ex)
			{

				return BadRequest(ex);
			}
		}

		[HttpGet]
		public async Task<IActionResult> News([FromQuery]BeersNewRegionQuery beersNewRegionQuery)
		{
			try
			{
				var beers = await QueryDispatcher.DispatchAsync<BeersNewRegionQuery, IEnumerable<BeersNewRegionQuery.Beer>>(beersNewRegionQuery);
				return Ok(beers);
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}


		//[HttpPost]
		//public async Task<IActionResult> CreateNewBeer(int productId)
		//{
		//	var beer = await _beerService.SaveBeerProdNumberAsync(productId);
		//	if (beer == null)
		//		return BadRequest("ProductId yielded no result");

		//	return Ok(beer);
		//}

	}
}