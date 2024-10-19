using AutoMapper;
using CookingAssistantAPI.Database.Models;
using CookingAssistantAPI.DTO.Recipes;
using CookingAssistantAPI.Exceptions;
using CookingAssistantAPI.Repositories;
using CookingAssistantAPI.Repositories.Recipes;
using CookingAssistantAPI.Services.UserServices;
using Microsoft.EntityFrameworkCore;

namespace CookingAssistantAPI.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IRepositoryRecipe _repository;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContext;
        public RecipeService(IRepositoryRecipe repository, IMapper mapper, IUserContextService userContext)
        {
            _repository = repository;
            _mapper = mapper;
            _userContext = userContext;
        }
        public async Task<bool> AddRecipe(RecipeCreateDTO recipeDto)
        {
            var recipe = _mapper.Map<Recipe>(recipeDto);
            recipe.CreatedById = _userContext.UserId;

            var nutrientData = recipe.RecipeNutrients
                .Zip(recipeDto.NutrientQuantities, (recipeNutrient, quantity) => new { recipeNutrient, quantity })
                .Zip(recipeDto.NutrientUnits, (pair, unit) => new { pair.recipeNutrient, pair.quantity, unit });

            var ingredientData = recipe.RecipeIngredients
                .Zip(recipeDto.IngredientQuantities, (recipeIngredient, quantity) => new { recipeIngredient, quantity })
                .Zip(recipeDto.IngredientUnits, (pair, unit) => new { pair.recipeIngredient, pair.quantity, unit });

            foreach (var data in nutrientData)
            {
                data.recipeNutrient.Quantity = data.quantity;
                data.recipeNutrient.Unit = data.unit;
            }

            foreach (var data in ingredientData)
            {
                data.recipeIngredient.Quantity = data.quantity;
                data.recipeIngredient.Unit = data.unit;
            }

            await _repository.AddRecipeAsync(recipe);

            if (await _repository.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteRecipeByIdAsync(int recipeId)
        {
            if(await _repository.DeleteRecipeByIdAsync(recipeId, _userContext.UserId))
            {
                return true;
            }
            return false;
        }

        public async Task<List<string>> GetAllIngredientsAsync()
        {
            return await _repository.GetAllIngredientsListAsync();
        }

        public async Task<List<string>> GetAllNutrientsAsync()
        {
            return await _repository.GetAllNutrientsListAsync();
        }

        public async Task<List<RecipeGetDTO>> GetAllRecipesAsync()
        {
            var recipes = await _repository.GetAllRecipesAsync();
            return _mapper.Map<List<RecipeGetDTO>>(recipes);
        }

        public async Task<RecipeGetDTO> GetRecipeByIdAsync(int recipeId)
        {
            var recipe = await _repository.GetRecipeByIdAsync(recipeId);
            return _mapper.Map<RecipeGetDTO>(recipe);
        }

        public async Task<RecipeGetDTO> GetRecipeByNameAsync(string recipeName)
        {
            var recipe = await _repository.GetRecipeByNameAsync(recipeName);
            return _mapper.Map<RecipeGetDTO>(recipe);
        }

        public async Task<byte[]> GetRecipeImageAsync(int recipeId)
        {
            var image = await _repository.GetRecipeImageAsync(recipeId);
            if (image == null)
            {
                throw new BadRequestException("This recipe does not have an image");
            }
            return image;
        }
    }
}
