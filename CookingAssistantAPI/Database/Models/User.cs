
namespace CookingAssistantAPI.Database.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public virtual Role? Role { get; set; }
        public virtual ICollection<Recipe>? CreatedRecipes { get; set; } = new List<Recipe>();
        public virtual ICollection<Recipe>? FavouriteRecipes { get; set; } = new List<Recipe>();
    }
}
