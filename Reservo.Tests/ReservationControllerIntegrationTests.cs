using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using ReservationServiceApi.Dtos;
using Xunit;

namespace ReservationServiceApi.IntegrationTests
{
    public class ReservationControllerTests : IClassFixture<WebApplicationFactory<ReservationServiceApi.Program>>
    {
        private readonly HttpClient _client;

        public ReservationControllerTests(WebApplicationFactory<ReservationServiceApi.Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetReservations_ShouldReturnOk()
        {
            var response = await _client.GetAsync("/reservations");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetCartContent_WithInvalidCustomerId_ShouldReturnNotFound()
        {
            var response = await _client.GetAsync("/cart/9999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task ConfirmCart_WithInvalidCustomerId_ShouldReturnBadRequest()
        {
            var response = await _client.PatchAsync("/cart/9999/confirm", null);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CompleteReservation_WithInvalidId_ShouldReturnBadRequest()
        {
            var response = await _client.PatchAsync("/reservation/9999/complete", null);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CancelReservation_WithInvalidId_ShouldReturnBadRequest()
        {
            var response = await _client.PatchAsync("/reservation/9999/cancel", null);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DeleteReservation_WithInvalidId_ShouldReturnNotFound()
        {
            var response = await _client.DeleteAsync("/reservation/9999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetRooms_ShouldReturnOk()
        {
            var response = await _client.GetAsync("/rooms");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task AddRoom_WithValidDto_ShouldReturnOkOrBadRequest()
        {
            var dto = new RoomDto
            {
                RoomNumber = 1,
                PricePerNight = 100
            };

            var response = await _client.PostAsJsonAsync("/room", dto);

            Assert.True(
                response.StatusCode == HttpStatusCode.OK ||
                response.StatusCode == HttpStatusCode.BadRequest
            );
        }

        [Fact]
        public async Task DeleteRoom_WithInvalidId_ShouldReturnBadRequest()
        {
            var response = await _client.DeleteAsync("/room/9999");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
