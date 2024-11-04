using CookingAssistantAPI.DTO.Recipes;
using CookingAssistantAPI.Tools;

namespace CookingAssistantAPI.Services.RecipeServices
{
    public interface IRecipeQueryService
    {
        List<RecipeSimpleGetDTO> SearchRecipes(ref List<RecipeSimpleGetDTO> recipeDtos, string? searchPhrase);
        List<RecipeSimpleGetDTO> SortRecipes(ref List<RecipeSimpleGetDTO> recipeDtos, SortBy? sortBy, SortDirection? sortDirection);
        List<RecipeSimpleGetDTO> RecipeFilter(ref List<RecipeSimpleGetDTO> recipeDtos, string? categoryName, string? difficulty, string? occasion);
        Dictionary<string, Func<RecipeSimpleGetDTO, object>> SortSelector { get; }
        Dictionary<string, int> DifficultyOrder { get; }
    }
}
