using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using CustomerServiceApi.Dtos;

namespace CustomerServiceApi.IntegrationTests
{

    public class CustomerControllerIntegrationTests : IClassFixture<WebApplicationFactory<CustomerServiceApi.Program>>
    {
        private readonly HttpClient _client;

        public CustomerControllerIntegrationTests(WebApplicationFactory<CustomerServiceApi.Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllCustomers_ShouldReturnOk()
        {
            // Act
            var response = await _client.GetAsync("/customers");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetCustomerById_WhenNotExists_ShouldReturnNotFound()
        {
            // Act
            var response = await _client.GetAsync("/customers/99999");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateCustomer_WithValidDto_ShouldReturnOk()
        {
            // Arrange
            var dto = new CustomerDto
            {
                FullName = "Test User",
                Info = new CustomerInfoDto
                {
                    Email = "test@example.com",
                    PhoneNumber = "123456789",
                    IsVIP = true
                }
            };

            // Act
            var response = await _client.PostAsJsonAsync("/customer", dto);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeleteCustomer_WhenNotExists_ShouldReturnNotFound()
        {
            // Act
            var response = await _client.DeleteAsync("/customers/99999");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateCustomer_WhenNotExists_ShouldReturnNotFound()
        {
            // Arrange
            var dto = new CustomerDto
            {
                FullName = "Updated Name",
                Info = new CustomerInfoDto
                {
                    Email = "updated@example.com",
                    PhoneNumber = "000111222",
                    IsVIP = false
                }
            };

            // Act
            var response = await _client.PatchAsJsonAsync("/customers/99999", dto);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
