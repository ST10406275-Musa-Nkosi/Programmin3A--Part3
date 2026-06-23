using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using TechMoveGLMS.API; // Adjust this namespace to match your Web API project definition

namespace TechMoveGLMS.Tests
{
    public class ContractsApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ContractsApiIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetContracts_WithoutToken_Returns401Unauthorized()
        {
            // Arrange: Create an unauthenticated client targeting the running test server instance
            var client = _factory.CreateClient();

            // Act: Fire a request straight to the protected endpoint path
            var response = await client.GetAsync("api/ContractsApi");

            // Assert: Verify that security middleware intercepted the request and blocked access safely
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetContracts_WithValidToken_Returns200OKAndJsonPayload()
        {
            // Arrange: Setup the testing client infrastructure
            var client = _factory.CreateClient();

            // 1. Authenticate against the AuthController login route to request a valid token
            var loginPayload = new { Username = "admin", Password = "Password123!" };
            var loginResponse = await client.PostAsJsonAsync("api/Auth/login", loginPayload);
            loginResponse.EnsureSuccessStatusCode();

            var authResult = await loginResponse.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            Assert.NotNull(authResult);
            Assert.True(authResult.ContainsKey("token"));

            // 2. Inject the extracted bearer token into the default request headers collection
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult["token"]);

            // Act: Fire the request to the secure Contracts endpoint with the validation token attached
            var response = await client.GetAsync("api/ContractsApi");

            // Assert: Verify the pipeline processed the request and data returned smoothly
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            Assert.NotNull(content);
            Assert.StartsWith("[", content); // Verifies that the payload returned a structured JSON array list
        }
    }
}