using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CookingAssistantAPI.Tools
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SortBy
    {
        Ratings,
        TimeInMinutes,
        DifficultyName,
        VoteCount,
        CategoryName,
        Caloricity
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SortDirection
    {
        Ascending,
        Descending
    }

    public class RecipeQuery
    {
        public string? SearchPhrase { get; set; }
        public string? IngredientsSearch {  get; set; }
        public SortBy? SortBy { get; set; }
        public SortDirection? SortDirection { get; set; }
        public string? FilterByDifficulty { get; set; }
        public string? FilterByCategoryName { get; set; }
        public string? FilterByOccasion { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}
