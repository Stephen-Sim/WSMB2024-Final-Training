using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace App1.Services
{
    public static class ApiService<T>
    {
        public static async Task<T> Get(string @params)
        {
            var url = "http://10.131.76.121:8090/api/values/" + @params;
            using (HttpClient client = new HttpClient())
            {
                var res = await client.GetStringAsync(url);
                return JsonConvert.DeserializeObject<T>(res);   
            }
        }
    }
}
