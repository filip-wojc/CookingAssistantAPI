using AutoMapper;
using CookingAssistantAPI.Database.Models;
using CookingAssistantAPI.DTO;
using CookingAssistantAPI.Tools.Converters;

namespace CookingAssistantAPI.Tools
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<IFormFile, byte[]>().ConvertUsing<FormFileToByteArrayConverter>();

            CreateMap<NutrientCreateDTO, Nutrient>();

            CreateMap<RecipeCreateDTO, Recipe>()
                .ForMember(dest => dest.ImageData, o => o.MapFrom(src => src.ImageData))
                .ForMember(dest => dest.Steps, o => o.MapFrom(src => MapSteps(src.Steps)))
                .ForMember(dest => dest.RecipeIngredients, o => o.MapFrom(src => MapRecipeIngredients(src.IngredientNames)))
                .ForMember(dest => dest.RecipeNutrients, o => o.MapFrom(src => MapRecipeNutrients(src.NutrientNames)));

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
                Nutrient = new Nutrient { NutrientName = name}
            }).ToList();
        }

    }
}
