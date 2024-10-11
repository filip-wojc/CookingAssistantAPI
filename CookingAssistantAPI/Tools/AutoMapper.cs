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
                .ForMember(dest => dest.Ingredients, o => o.MapFrom(src => MapIngredients(src.IngredientNames)));
                
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

        private ICollection<Ingredient> MapIngredients(ICollection<string>? ingredientNames)
        {
            if (ingredientNames == null) return new List<Ingredient>();

            return ingredientNames.Select(name => new Ingredient
            {
                IngredientName = name
                
            }).ToList();
        }
    }
}
