﻿using CookingAssistantAPI.Database.Models;
using CookingAssistantAPI.DTO.Recipes;
using CookingAssistantAPI.Tools;

namespace CookingAssistantAPI.Services.RecipeServices
{
    public interface IRecipeService
    {
        Task<bool> AddRecipe(RecipeCreateDTO recipeDto);
        Task<RecipeGetDTO> GetRecipeByIdAsync(int recipeId);
        Task<RecipeGetDTO> GetRecipeByNameAsync(string recipeName);
        Task<List<RecipeSimpleGetDTO>> GetAllRecipesAsync(RecipeQuery query);
        Task<List<RecipeNamesGetDTO>> GetAllRecipesNamesAsync();
        Task<byte[]> GetRecipeImageAsync(int recipeId);
        Task<List<string>> GetAllNutrientsAsync();
        Task<List<string>> GetAllIngredientsAsync();
        Task<bool> DeleteRecipeByIdAsync(int recipeId);
    }
}