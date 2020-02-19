using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MyBeers.Api.Utils
{
    public class BuildImageUrls
    {
        public static string BuildUrl(int id)
        {
            var url = $"https://sb-product-media-prod.azureedge.net/productimages/{id}/{id}_200.png";
            if (IsValidUrl(url))
            {
                return url;
            }

            return null;
        }

        private static bool IsValidUrl(string url)
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
