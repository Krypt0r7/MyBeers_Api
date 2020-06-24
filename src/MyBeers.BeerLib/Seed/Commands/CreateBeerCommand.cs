using MongoDB.Driver.Core.Misc;
using MyBeers.BeerLib.Domain;
using MyBeers.Common.CommonInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyBeers.BeerLib.Seed.Commands
{
    public class CreateBeerCommand : ICommand
    {
        public string Name { get; }
        public string Producer { get; }
        public string Container { get; }
        public double RecycleFee { get; }
        public double AlcoholPercentage { get; }
        public double Volume { get; }
        public double Price { get; }
        public double YPK { get; }
        public string Country { get; }
        public string State { get; }
        public string City { get; }
        public string Type { get; }
        public string Style { get; }
        public string ProductionScale { get; }
        public string Usage { get; }
        public string Taste { get; }
        public long ProductIdSystemet { get; }
        public SystemetInformation SystemetInformation { get; }
        public DateTime SellStartSystemet { get; set; }
        public string ImageUrl { get; }

        public CreateBeerCommand(string name, string producer, string container, double recycleFee, double alcoholPrecentage, double volume, double price, double ypk, string country, string state, string city, string type, string style, string scale, string usage, string taste, long sysProdId, SystemetInformation systemetInformation, DateTime dateTime, string imageUrl)
        {
            Name = name;
            Producer = producer;
            Container = container;
            RecycleFee = recycleFee;
            AlcoholPercentage = alcoholPrecentage;
            Volume = volume;
            Price = price;
            YPK = ypk;
            Country = country;
            State = state;
            City = city;
            Type = type;
            Style = style;
            ProductionScale = scale;
            Usage = usage;
            Taste = taste;
            ProductIdSystemet = sysProdId;
            SystemetInformation = systemetInformation;
            SellStartSystemet = dateTime;
            ImageUrl = imageUrl;
        }
    }
}
