using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using CookingAssistantAPI.Database;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.InMemory;

namespace CookingAssistantAPI.IntegrationTests
{
    public class ResourceControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public ResourceControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services.SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<CookingDbContext>));
                        services.Remove(dbContextOptions);

                        services.AddDbContext<CookingDbContext>(o => o.UseSqlite("Data Source=recipes_tests.db"));
                        
                    });
                })
                .CreateClient();
        }

        [Fact]
        public async Task GetAllIngredientNames_ReturnsOkResult()
        {
            // act

            var response = await _client.GetAsync("api/resources/ingredients");

            // assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        }

        [Fact]
        public async Task GetAllUnits_ReturnsOkResult()
        {  
            // act

            var response = await _client.GetAsync("api/resources/units");

            // assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        }

        [Fact]
        public async Task GetAllDifficulties_ReturnsOkResult()
        {
            // act

            var response = await _client.GetAsync("api/resources/difficulties");

            // assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        }

        [Fact]
        public async Task GetAllOccasions_ReturnsOkResult()
        {
            // act

            var response = await _client.GetAsync("api/resources/occasions");

            // assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        }
    }
}
