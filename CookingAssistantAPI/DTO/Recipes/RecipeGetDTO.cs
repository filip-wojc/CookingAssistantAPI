﻿using CookingAssistantAPI.DTO.RecipeIngredients;
using CookingAssistantAPI.DTO.RecipeNutrients;
using CookingAssistantAPI.DTO.Steps;

namespace CookingAssistantAPI.DTO.Recipes
{
    public class RecipeGetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AuthorName { get; set; }
        public byte[] ImageData { get; set; }
        public float Ratings { get; set; }
        public int TimeInMinutes { get; set; }
        public int Serves { get; set; }
        public string? Difficulty { get; set; }
        public int VoteCount { get; set; }
        public string CategoryName { get; set; }
        public List<RecipeIngredientGetDTO>? Ingredients { get; set; }
        public List<RecipeNutrientGetDTO>? Nutrients { get; set; }
        public List<StepGetDTO>? Steps { get; set; }

    }
}