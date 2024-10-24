using CookingAssistantAPI.Database.Models;
using CookingAssistantAPI.DTO.Recipes;
using CookingAssistantAPI.Tools;
using System.Linq;

namespace CookingAssistantAPI.Services.RecipeServices
{
    public class RecipePaginationService : IRecipePaginationService
    {
        public PageResult<RecipeSimpleGetDTO> GetPaginatedResult(RecipeQuery query, IEnumerable<RecipeSimpleGetDTO> allEntities)
        {
            if (query.PageNumber != null && query.PageSize != null)
            {
                var recipes = allEntities.Skip((int)query.PageSize * (int)(query.PageNumber - 1)).Take((int)query.PageSize).ToList();
                var totalRecipes = allEntities.Count();
                return new PageResult<RecipeSimpleGetDTO>(recipes, totalRecipes, (int)query.PageSize, (int)query.PageNumber);
            }
            return new PageResult<RecipeSimpleGetDTO>(allEntities.ToList(), allEntities.Count(), allEntities.Count(), 1);

        }
    }
}
