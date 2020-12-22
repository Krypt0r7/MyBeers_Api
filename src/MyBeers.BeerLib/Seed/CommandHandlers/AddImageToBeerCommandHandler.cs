using MyBeers.BeerLib.Api.Commands;
using MyBeers.BeerLib.Domain;
using MyBeers.Common.Bases;
using MyBeers.Common.Dispatchers;
using MyBeers.Common.MongoSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MyBeers.BeerLib.Seed.CommandHandlers
{
	public class AddImageToBeerCommandHandler : BaseCommandHandler<AddImageToBeerCommand, Domain.Beer>
	{
		public AddImageToBeerCommandHandler(IMongoRepository<Beer> repository, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher) : base(repository, queryDispatcher, commandDispatcher)
		{
		}

		public override async Task HandleAsync(AddImageToBeerCommand command)
		{
			var beers = await Repository.FilterByAsync(filter => true);
			try
			{
				await AddImageToBeers(beers);
			}
			catch (Exception)
			{
				
			}
		}

		private async Task AddImageToBeers(IEnumerable<Domain.Beer> beers)
		{
			foreach (var beer in beers)
			{
				if (beer.ImageUrl == null)
				{
					string imageUrl = BuildUrl(beer.Containers.First().ProductIdFromSystmet);
					if (imageUrl != null)
					{
						Console.WriteLine(imageUrl);
						//beer.ImageUrl = imageUrl;
						await Repository.ReplaceAsync(beer);
					}
					
				}
			}
		}

		private string BuildUrl(int id)
		{
			var url = $"https://sb-product-media-prod.azureedge.net/productimages/{id}/{id}_200.png";
			if (IsValidUrl(url))
			{
				return url;
			}

			return null;
		}

		private bool IsValidUrl(string url)
		{
			var uri = new UriBuilder(url);
			var req = WebRequest.Create(uri.Uri);
			req.Method = "HEAD";
			try
			{
				using (var resp = req.GetResponse())
				{
					return true;
				}
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
