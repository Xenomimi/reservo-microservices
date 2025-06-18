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

                HttpResponseMessage response = await client.GetAsync($"discounts/code/{code}");
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response);
                    return null;
                }

                string responseData = await response.Content.ReadAsStringAsync();
                var discount = JsonConvert.DeserializeObject<DiscountDto>(responseData);
                Console.WriteLine(discount.Percentage);
                return discount;
            }
        }

        public async Task MarkDiscountAsUsed(int id)
        {
            string apiUrl = "http://localhost:5107/";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.PatchAsync($"discounts/{id}/mark-as-used", null);

                if (!response.IsSuccessStatusCode)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Nie udało się oznaczyć rabatu jako użyty. Kod: {response.StatusCode}, Treść: {body}");
                }
            }
        }
    }
}
