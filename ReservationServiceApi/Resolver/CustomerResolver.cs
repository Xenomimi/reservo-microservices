using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace ReservationServiceApi.Resolver
{
    public class CustomerResolver
    {
        public async Task<string?> ResolveFor(int customerId)
        {
            return await ResolveFromExternalDictionary(customerId);
        }

        private async Task<string?> ResolveFromExternalDictionary(int customerId)
        {
            string apiUrl = "http://localhost:5001/";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("api/customers");
                string responseData = await response.Content.ReadAsStringAsync();

                var customers = JsonConvert.DeserializeObject<List<CustomerDto>>(responseData);
                return customers?.FirstOrDefault(x => x.Id == customerId)?.FullName;
            }
        }

        public class CustomerDto
        {
            public int Id { get; set; }
            public string FullName { get; set; } = null!;
        }
    }
}
