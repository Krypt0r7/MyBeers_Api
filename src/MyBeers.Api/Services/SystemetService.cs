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

        public async Task<List<BeerData>> GetNews(string region)
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

                var mappedBeer = _mapper.Map<List<BeerData>>(objects.Hits);

                return mappedBeer;
            }
            return new List<BeerData>();
        }

        public async Task<BeerData> SearchSingleBeer(int id)
        {
            var response = await HttpClient.GetAsync($"product/v1/product/{id}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var systemetData = JsonConvert.DeserializeObject<SystemetBeerIn>(data);
                var mappedBeer = _mapper.Map<BeerData>(systemetData);
                mappedBeer.ImageUrl = BuildImageUrls.BuildUrl((int)mappedBeer.ProductId);
                return mappedBeer;
            }

            return null;
        }

        public async Task<List<BeerData>> SearchSystemetAsync(string searchString)
        {
            var timer = new Stopwatch();
            timer.Start();
            var response = await HttpClient.GetAsync($"product/v1/product/search?SearchQuery={searchString}&Category=Öl&SubCategory=Öl");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var objects = JsonConvert.DeserializeObject<BeerListModel>(data);
                var mappedBeers = _mapper.Map<List<BeerData>>(objects.Hits);

                //foreach (var item in mappedBeers)
                //{
                //    item.ImageUrl = BuildImageUrls.BuildUrl((int)item.ProductId);
                //}

                timer.Stop();
                var elapsed = timer.ElapsedMilliseconds;
                
                    return mappedBeers;
            }



            return null;
        }
    }
}
