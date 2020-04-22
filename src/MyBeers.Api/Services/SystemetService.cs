using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using MyBeers.Api.Data;
using MyBeers.Api.DataSystemet;
using MyBeers.Api.Utils;
using Newtonsoft.Json;

namespace MyBeers.Api.Services
{
    public class SystemetService : ISystemetService
    {
        private readonly IMapper _mapper;
        private readonly HttpClient HttpClient = new HttpClient();
        public SystemetService(IMapper mapper)
        {
            _mapper = mapper;
            HttpClient.BaseAddress = new Uri("https://api-extern.systembolaget.se/");
            HttpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "ce26456ac64b43a38dbc40dc6925177c");
        }

        public async Task<List<Beer.BeerDataModel>> GetNews(string region)
        {
            string regionText = (region != null) ? "&originLevel1=" + region : "";
            var today = DateTime.UtcNow.Date;
            var twoWeeksFromNow = DateTime.UtcNow.AddDays(14).Date;
            string url = $"product/v1/product/search?Country=Sverige&SubCategory=Öl&SellStartDateFrom={today}&SellStartDateTo={twoWeeksFromNow}{regionText}";
            var response = await HttpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var objects = JsonConvert.DeserializeObject<BeerListModel>(data);

                var mappedBeer = _mapper.Map<List<Beer.BeerDataModel>>(objects.Hits);

                return mappedBeer;
            }
            return new List<Beer.BeerDataModel>();
        }

        public async Task<Beer.BeerDataModel> SearchSingleBeer(int id)
        {
            var response = await HttpClient.GetAsync($"product/v1/product/{id}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var systemetData = JsonConvert.DeserializeObject<SystemetBeerIn>(data);
                var mappedBeer = _mapper.Map<Beer.BeerDataModel>(systemetData);
                mappedBeer.ImageUrl = BuildImageUrls.BuildUrl((int)mappedBeer.ProductId);
                return mappedBeer;
            }

            return null;
        }

        public async Task<List<Beer.BeerDataModel>> SearchSystemetAsync(string searchString)
        {
            var response = await HttpClient.GetAsync($"product/v1/product/search?SearchQuery={searchString}&Category=Öl&SubCategory=Öl");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var objects = JsonConvert.DeserializeObject<BeerListModel>(data);
                var mappedBeers = _mapper.Map<List<Beer.BeerDataModel>>(objects.Hits);
                
                return mappedBeers;
            }

            return null;
        }
    }
}
