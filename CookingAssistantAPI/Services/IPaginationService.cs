using CookingAssistantAPI.DTO.Recipes;
using CookingAssistantAPI.Tools;

namespace CookingAssistantAPI.Services
{
    public interface IPaginationService
    {
        public PageResult<RecipeSimpleGetDTO> GetPaginatedResult(RecipeQuery query, IEnumerable<RecipeSimpleGetDTO> allEntities);
    }
}
