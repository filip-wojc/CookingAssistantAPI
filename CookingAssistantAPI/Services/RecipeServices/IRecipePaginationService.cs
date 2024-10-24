using CookingAssistantAPI.DTO.Recipes;
using CookingAssistantAPI.Tools;

namespace CookingAssistantAPI.Services.RecipeServices
{
    public interface IRecipePaginationService
    {
        public PageResult<RecipeSimpleGetDTO> GetPaginatedResult(RecipeQuery query, IEnumerable<RecipeSimpleGetDTO> allEntities);
    }
}
