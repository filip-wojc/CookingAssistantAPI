namespace CookingAssistantAPI.Database.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public string? Description { get; set; }
        public DateTime DateCreated {  get; set; }
        public DateTime? DateModified { get; set; }
        public virtual User? ReviewAuthor { get; set; }
        public int? ReviewAuthorId { get; set; }
        public virtual Recipe? RatedRecipe { get; set; }
        public int RatedRecipeId { get; set; }
    }
}
