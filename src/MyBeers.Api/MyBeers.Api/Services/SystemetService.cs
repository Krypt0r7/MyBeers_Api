using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyBeers.Api.Services
{
    public class SystemetService : ISystemetService
    {
        public async Task<string> SearchSystemetAsync(string searchString)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://api-extern.systembolaget.se/")
            };

            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "ce26456ac64b43a38dbc40dc6925177c");

            var response = await client.GetAsync($"product/v1/product/search?SearchQuery={searchString}&Category=öl");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return data;
            }

            return "failed";
        }
    }
}
