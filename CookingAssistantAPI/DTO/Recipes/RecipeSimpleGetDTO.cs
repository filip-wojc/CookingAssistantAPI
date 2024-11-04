namespace CookingAssistantAPI.DTO.Recipes
{
    public class RecipeSimpleGetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Ratings { get; set; }
        public int TimeInMinutes { get; set; }
        public string Difficulty { get; set; }
        public int VoteCount { get; set; }
        public int Caloricity { get; set; }
        public string Occasion { get; set; }
        public string CategoryName { get; set; }
        public List<string> IngredientNames { get; set; }
    }
}
