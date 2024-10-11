namespace CookingAssistantAPI.Database.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public virtual User? CreatedBy { get; set; }
        public int CreatedById { get; set; }
        public byte[]? ImageData { get; set; }
        public float Ratings { get; set; }
        public int? TimeInMinutes {  get; set; }
        public int Serves { get; set; }
        public string? Difficulty { get; set; }
        public int VoteCount { get; set; }
        public virtual Category? Category { get; set; }
        public int CategoryId { get; set; }
        public virtual ICollection<Ingredient>? Ingredients { get; set; }
        public virtual ICollection<Step>? Steps { get; set; }
        public virtual ICollection<Nutrient>? Nutrients { get; set; }
        public virtual ICollection<User>? UsersFavourite { get; set; }
    }
}
