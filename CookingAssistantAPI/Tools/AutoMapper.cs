using AutoMapper;
using CookingAssistantAPI.Database.Models;
using CookingAssistantAPI.DTO.RecipeIngredients;
using CookingAssistantAPI.DTO.RecipeNutrients;
using CookingAssistantAPI.DTO.Recipes;
using CookingAssistantAPI.DTO.Reviews;
using CookingAssistantAPI.DTO.Steps;
using CookingAssistantAPI.DTO.Users;
using CookingAssistantAPI.Tools.Converters;

namespace CookingAssistantAPI.Tools
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<IFormFile, byte[]>().ConvertUsing<FormFileToByteArrayConverter>();

            CreateMap<Step, StepGetDTO>();
            CreateMap<RecipeIngredient, RecipeIngredientGetDTO>()
                .ForMember(r => r.IngredientName, o => o.MapFrom(src => src.Ingredient.IngredientName));
            CreateMap<RecipeNutrient, RecipeNutrientGetDTO>()
                .ForMember(r => r.NutrientName, o => o.MapFrom(src => src.Nutrient.NutrientName));

            CreateMap<Recipe, RecipeGetDTO>()
                .ForMember(r => r.AuthorName, o => o.MapFrom(src => src.CreatedBy.UserName))
                .ForMember(r => r.CategoryName, o => o.MapFrom(src => src.Category.Name))
                .ForMember(r => r.Ingredients, o => o.MapFrom(src => src.RecipeIngredients))
                .ForMember(r => r.Nutrients, o => o.MapFrom(src => src.RecipeNutrients))
                .ForMember(r => r.Steps, o => o.MapFrom(src => src.Steps));

            CreateMap<Recipe, RecipeSimpleGetDTO>()
                .ForMember(r => r.CategoryName, o => o.MapFrom(src => src.CreatedBy.UserName));


            CreateMap<RecipeCreateDTO, Recipe>()
                //.ForMember(dest => dest.ImageData, o => o.MapFrom(src => src.ImageData))
                .ForMember(dest => dest.Steps, o => o.MapFrom(src => MapSteps(src.Steps)))
                .ForMember(dest => dest.RecipeIngredients, o => o.MapFrom(src => MapRecipeIngredients(src.IngredientNames)))
                .ForMember(dest => dest.RecipeNutrients, o => o.MapFrom(src => MapRecipeNutrients(src.NutrientNames)));

            CreateMap<ReviewCreateDTO, Review>();

        }

        private ICollection<Step> MapSteps(ICollection<string>? steps)
        {
            if (steps == null) return new List<Step>();

            return steps.Select((step, index) => new Step
            {
                StepNumber = index + 1,
                Description = step
            }).ToList();
        }

        private ICollection<RecipeIngredient> MapRecipeIngredients(ICollection<string>? ingredientNames)
        {
            if (ingredientNames == null) return new List<RecipeIngredient>();

            // Map the ingredient names to RecipeIngredient objects
            return ingredientNames.Select(name => new RecipeIngredient
            {
                Ingredient = new Ingredient { IngredientName = name } // Creating new Ingredient objects
            }).ToList();
        }

        private ICollection<RecipeNutrient> MapRecipeNutrients(ICollection<string>? nutrientNames)
        {
            if (nutrientNames == null) return new List<RecipeNutrient>();

            // Map the NutrientCreateDTO to RecipeNutrient objects
            return nutrientNames.Select(name => new RecipeNutrient
            {
                Nutrient = new Nutrient
                {
                    NutrientName = name,
                },
            }).ToList();
        }

    }
}
