using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;
using ReservationServiceApi;

namespace Reservo.Tests
{
    public class RoomIntegrationTests : IClassFixture<CustomWebApplicationFactory<ReservationServiceApi.Program>>
    {
        private readonly HttpClient _client;

        public RoomIntegrationTests(CustomWebApplicationFactory<ReservationServiceApi.Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetRooms_ReturnsList()
        {
            var response = await _client.GetAsync("rooms");

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            Assert.Contains("roomNumber", content); // lub sprawdź JSON
        }
    }
}
