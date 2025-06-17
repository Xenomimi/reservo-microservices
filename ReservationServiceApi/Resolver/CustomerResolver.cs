using Newtonsoft.Json;
using ReservationServiceApi.Dtos;
using System.Net.Http.Headers;

namespace ReservationServiceApi.Resolver
{
    public class CustomerResolver
    {
        public async Task<CustomerDto?> ResolveCustomer(int customerId)
        {
            return await ResolveFromExternalDictionary(customerId);
        }

        private async Task<CustomerDto?> ResolveFromExternalDictionary(int customerId)
        {
            string apiUrl = "http://localhost:5044/";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("customers/{customerId}");
                if (!response.IsSuccessStatusCode)
                    return null;

                string responseData = await response.Content.ReadAsStringAsync();
                var customer = JsonConvert.DeserializeObject<CustomerDto>(responseData);

                return customer;
            }
        }
    }
}
