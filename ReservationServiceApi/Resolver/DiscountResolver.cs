using Newtonsoft.Json;
using ReservationServiceApi.Dtos;
using System.Net.Http.Headers;

namespace ReservationServiceApi.Resolver
{
    public class DiscountResolver
    {
        public async Task<DiscountDto?> ResolveDiscount(string code)
        {
            return await ResolveFromExternalDictionary(code);
        }

        public async Task<DiscountDto?> ResolveFromExternalDictionary(string code)
        {
            string apiUrl = "http://localhost:5107/";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("discounts/code/{code}");
                if (!response.IsSuccessStatusCode)
                    return null;

                string responseData = await response.Content.ReadAsStringAsync();
                var discount = JsonConvert.DeserializeObject<DiscountDto>(responseData);
                return discount;
            }
        }

        public async Task MarkDiscountAsUsed(int id)
        {
            string apiUrl = "http://localhost:5045/";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                await client.PatchAsync($"discounts/{id}/mark-used", null);
            }
        }
    }
}
