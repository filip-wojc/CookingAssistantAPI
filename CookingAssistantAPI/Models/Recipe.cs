namespace CookingAssistantAPI.Models
{
    public class Recipe
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string ImageUrl { get; set; }
        public string Url { get; set; }
        public float Ratings { get; set; }
        public int Serves { get; set; }
        public string Difficulty { get; set; }
        public int VoteCount { get; set; }
        public string Subcategory { get; set; }
        public string DishType { get; set; }
        public string Maincategory { get; set; }

        public List<Ingredient> Ingredients { get; set; }
        public List<Step> Steps { get; set; }
        public List<Nutrient> Nutrients { get; set; }
        public List<Time> Times { get; set; }
    }
}
