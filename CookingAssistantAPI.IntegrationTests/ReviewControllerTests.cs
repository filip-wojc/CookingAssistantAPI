using CookingAssistantAPI.Database;
using CookingAssistantAPI.DTO.Reviews;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;

namespace CookingAssistantAPI.IntegrationTests
{
    public class ReviewControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public ReviewControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services.SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<CookingDbContext>));
                        services.Remove(dbContextOptions);

                        services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                        services.AddMvc(o => o.Filters.Add(new FakeUserFilter()));

                        services.AddDbContext<CookingDbContext>(o => o.UseSqlite("Data Source=recipes_tests.db"));

                    });
                })
                .CreateClient();
        }


        [Theory]
        [InlineData("54")]
        [InlineData("21")]
        [InlineData("600")]
        public async Task CreateReview_WithValidModel_ReturnsCreatedStatus(string recipeId)
        {
            // arrange

            var model = new ReviewCreateDTO()
            {
                Value = 4,
                Description = "Test desc"
            };

            var json = JsonConvert.SerializeObject(model);

            var httpContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            
            // act

            var response = await _client.PostAsync($"api/reviews/{recipeId}", httpContent);

            // assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            if (response.StatusCode != System.Net.HttpStatusCode.Created)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
            }

        }

        
    }
}
