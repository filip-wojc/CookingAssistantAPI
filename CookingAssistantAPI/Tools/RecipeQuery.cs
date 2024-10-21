using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CookingAssistantAPI.Tools
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SortBy
    {
        Ratings,
        TimeInMinutes,
        Difficulty,
        VoteCount,
        CategoryName 
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
        public SortBy? SortBy { get; set; }
        public SortDirection? SortDirection { get; set; }
        public string? FilterByDifficulty { get; set; }
        public string? FilterByCategoryName { get; set; }
    }
}
