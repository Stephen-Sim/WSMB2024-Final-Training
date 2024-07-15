using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Session5APP.Services
{
    public static class SampleService <T>
    {
        public static async Task<T> Get(string url_params)
        {
            var url = "http://10.105.13.186:8090/api/values/" + url_params;

            using (HttpClient client = new HttpClient())
            {
                var jsonStr = await client.GetStringAsync(url);
                return JsonConvert.DeserializeObject<T>(jsonStr);
            }
        }
    }
}
