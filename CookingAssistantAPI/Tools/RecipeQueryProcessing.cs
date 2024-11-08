using CookingAssistantAPI.Database.Models;
using CookingAssistantAPI.Exceptions;

namespace CookingAssistantAPI.Tools
{
    public static class RecipeQueryProcessing
    {
        public static IQueryable<Recipe> Filter(IQueryable<Recipe> recipesQuery, RecipeQuery query)
        {
            // Filtrowanie
            if (!string.IsNullOrEmpty(query.FilterByCategoryName))
            {
                recipesQuery = recipesQuery.Where(r => r.Category.Name.ToLower() == query.FilterByCategoryName.ToLower());
            }
            if (!string.IsNullOrEmpty(query.FilterByDifficulty))
            {
                recipesQuery = recipesQuery.Where(r => r.Difficulty.Name.ToLower() == query.FilterByDifficulty.ToLower());
            }
            if (!string.IsNullOrEmpty(query.FilterByOccasion))
            {
                recipesQuery = recipesQuery.Where(r => r.Occasion.Name.ToLower() == query.FilterByOccasion.ToLower());
            }
            if(!recipesQuery.Any())
            {
                throw new NotFoundException("No recipe found with given filters");
            }
            return recipesQuery;
        }
        public static IQueryable<Recipe> Search(IQueryable<Recipe> recipesQuery, RecipeQuery query)
        {
            if (!string.IsNullOrEmpty(query.SearchPhrase))
            {
                recipesQuery = recipesQuery.Where(r =>
                r.Name.ToLower().Contains(query.SearchPhrase.ToLower()) ||
                    r.Description.ToLower().Contains(query.SearchPhrase.ToLower()));
            }

            if (!string.IsNullOrEmpty(query.IngredientsSearch))
            {
                var ingredients = query.IngredientsSearch.Split(',').Select(i => i.ToLower()).ToList();
                recipesQuery = recipesQuery.Where(r =>
                    r.RecipeIngredients.Any(i => ingredients.Contains(i.Ingredient.IngredientName.ToLower())));
            }

            if (!recipesQuery.Any())
            {
                throw new NotFoundException("No recipe found with given searching");
            }

            return recipesQuery;
        }
        public static IQueryable<Recipe> Sort(IQueryable<Recipe> recipesQuery, RecipeQuery query)
        {
            if (query.SortBy != null && query.SortDirection != null)
            {
                recipesQuery = query.SortBy switch
                {
                    SortBy.Ratings => query.SortDirection == SortDirection.Ascending
                        ? recipesQuery.OrderBy(r => r.Ratings)
                        : recipesQuery.OrderByDescending(r => r.Ratings),
                    SortBy.TimeInMinutes => query.SortDirection == SortDirection.Ascending
                        ? recipesQuery.OrderBy(r => r.TimeInMinutes)
                        : recipesQuery.OrderByDescending(r => r.TimeInMinutes),
                    SortBy.Caloricity => query.SortDirection == SortDirection.Ascending
                        ? recipesQuery.OrderBy(r => r.Caloricity)
                        : recipesQuery.OrderByDescending(r => r.Caloricity),
                    SortBy.VoteCount => query.SortDirection == SortDirection.Ascending
                        ? recipesQuery.OrderBy(r => r.VoteCount)
                        : recipesQuery.OrderByDescending(r => r.VoteCount),
                    SortBy.Difficulty => query.SortDirection == SortDirection.Ascending
                        ? recipesQuery.OrderBy(r => r.Difficulty.Id)
                        : recipesQuery.OrderByDescending(r => r.Difficulty.Id),
                    _ => recipesQuery
                };
            }

            return recipesQuery;
        }
      
    }
}
