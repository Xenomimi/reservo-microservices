using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using ReservationServiceApi.Dtos;
using Xunit;
using DiscountServiceApi;

namespace DiscountServiceApi.IntegrationTests
{
    public class DiscountControllerIntegrationTests : IClassFixture<WebApplicationFactory<DiscountServiceApi.Program>>
    {
        private readonly HttpClient _client;

        public DiscountControllerIntegrationTests(WebApplicationFactory<DiscountServiceApi.Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllDiscounts_ShouldReturnOk()
        {
            var response = await _client.GetAsync("/discounts");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetDiscountByCode_WhenNotExists_ShouldReturnNotFound()
        {
            var response = await _client.GetAsync("/discounts/code/NONEXISTENT");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateDiscount_WithValidDto_ShouldReturnOk()
        {
            var discountDto = new DiscountDto
            {
                Code = "PROMOCJA",
                DiscountStatus = 0,
                Percentage = 10,
                RequiresVipCustomer = false
            };

            var response = await _client.PostAsJsonAsync("/discount", discountDto);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task MarkAsUsed_WithInvalidId_ShouldReturnNotFound()
        {
            var response = await _client.PatchAsync("/discounts/9999/mark-as-used", null);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteDiscount_WithInvalidId_ShouldReturnNotFound()
        {
            var response = await _client.DeleteAsync("/discounts/9999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
