using CookingAssistantAPI.Database;
using CookingAssistantAPI.DTO.Recipes;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;

namespace CookingAssistantAPI.IntegrationTests
{
    public class RecipeControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public RecipeControllerTests(WebApplicationFactory<Program> factory)
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

        [Fact]
        public async Task CreateRecipe_WithValidModel_ReturnsCreatedStatus()
        {
            // arrange
            var formData = new MultipartFormDataContent
             {
                 { new StringContent("Test Recipe"), "Name" },
                 { new StringContent("Test Description"), "Description" },
                 { new StringContent("10"), "Serves" },
                 { new StringContent("1"), "DifficultyId" },
                 { new StringContent("13"), "CategoryId" },
                 { new StringContent("1"), "OccasionId" },
                 { new StringContent("100"), "Caloricity" },
                 { new StringContent("10"), "TimeInMinutes" },


                 { new StringContent("testName1"), "IngredientNames" },
                 { new StringContent("testName2"), "IngredientNames" },
                 { new StringContent("10"), "IngredientQuantities" },
                 { new StringContent("20"), "IngredientQuantities" },
                 { new StringContent("kg"), "IngredientUnits" },
                 { new StringContent("g"), "IngredientUnits" },
                 { new StringContent("Step1"), "Steps" },
                 { new StringContent("Step2"), "Steps" }
             };

            // act

            var response = await _client.PostAsync("api/recipes", formData);

            // assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            if (response.StatusCode != System.Net.HttpStatusCode.Created)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content); 
            }

        }

        [Fact]
        public async Task CreateRecipe_WithValidModel_ReturnsBadRequestStatus()
        {
            // arrange
            var formData = new MultipartFormDataContent
             {
                 { new StringContent("Test Recipe"), "Name" },
                 { new StringContent("Test Description"), "Description" },
                 { new StringContent("10"), "Serves" },
                 { new StringContent("1"), "DifficultyId" },
                 { new StringContent("1"), "CategoryId" }, // Category does not exist
                 { new StringContent("1"), "OccasionId" },
                 { new StringContent("100"), "Caloricity" },
                 { new StringContent("10"), "TimeInMinutes" },


                 // Listy jako wiele powtórzeń kluczy
                 { new StringContent("testName1"), "IngredientNames" },
                 { new StringContent("testName2"), "IngredientNames" },
                 { new StringContent("10"), "IngredientQuantities" },
                 { new StringContent("20"), "IngredientQuantities" },
                 { new StringContent("kg"), "IngredientUnits" },
                 { new StringContent("g"), "IngredientUnits" },
                 { new StringContent("Step1"), "Steps" },
                 { new StringContent("Step2"), "Steps" }
             };

            // act

            var response = await _client.PostAsync("api/recipes", formData);

            // assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

            if (response.StatusCode != System.Net.HttpStatusCode.BadRequest)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
            }

        }

        [Theory]
        [InlineData("5")]
        [InlineData("50")]
        [InlineData("500")]
        public async Task GetRecipeById_ReturnsOkStatus(string recipeId)
        {
            // act

            var response = await _client.GetAsync($"api/recipes/{recipeId}");

            // assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("-1")]
        [InlineData("0")]
        public async Task GetRecipeById_ReturnsNotFoundStatus(string recipeId)
        {
            // act

            var response = await _client.GetAsync($"api/recipes/{recipeId}");

            // assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }



    }
}
