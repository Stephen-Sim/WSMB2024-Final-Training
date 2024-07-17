using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Session5APP.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Session5APP.Services
{
    public class APIService
    {
        public string URL { get; set; } = "http://10.105.13.186:8090/api/values/";
        public HttpClient client { get; set; }

        public APIService()
        {
            client = new HttpClient();
        }

        public async Task<bool> Login(string text1, string text2)
        {
            var url = this.URL + $"Login?u={text1}&p={text2}";

            var res = await client.GetAsync(url);

            if (res.IsSuccessStatusCode)
            {
                var jsonStr = await res.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<User>(jsonStr);
                App.User = user;
                return true;
            }

            return false;
        }

        public async Task<List<ServiceType>> GetServiceTypes()
        {
            var url = this.URL + $"GetServiceTypes";
            var res = await client.GetStringAsync(url);
            return JsonConvert.DeserializeObject<List<ServiceType>>(res);
        }

        public async Task<List<Service>> GetServices(long ID)
        {
            var url = this.URL + $"GetServices?ID={ID}";
            var res = await client.GetStringAsync(url);
            return JsonConvert.DeserializeObject<List<Service>>(res);
        }

        public async Task<int> GetRemainingSpot(long ID, DateTime dateTime)
        {
            var url = this.URL + $"GetRemainingSpot?ID={ID}&dateTime={dateTime}";
            var res = await client.GetStringAsync(url);
            return JsonConvert.DeserializeObject<int>(res);
        }

        public async Task<List<Coupon>> GetCoupons()
        {
            var url = this.URL + $"GetCoupons";
            var res = await client.GetStringAsync(url);
            return JsonConvert.DeserializeObject<List<Coupon>>(res);
        }

        public async Task<bool> StoreAddonService(AddonServiceStoreRequest addonRequest)
        {
            var url = this.URL + $"StoreAddonService";

            var json = JsonConvert.SerializeObject(addonRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var res = await client.PostAsync(url, content);

            if (res.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
    }
}

