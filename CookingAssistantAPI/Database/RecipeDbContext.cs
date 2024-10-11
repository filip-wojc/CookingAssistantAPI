using CookingAssistantAPI.Database.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;

namespace CookingAssistantAPI.Database
{
    public class RecipeDbContext : DbContext
    {
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Step> Steps { get; set; }
        public DbSet<Nutrient> Nutrients { get; set; }
        public DbSet<Time> Times { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=recipes.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
           .HasMany(u => u.CreatedRecipes)
           .WithOne(r => r.CreatedBy)
           .HasForeignKey(r => r.CreatedById);

            // Relacja wiele-do-wielu: Użytkownicy -> Ulubione przepisy
            modelBuilder.Entity<User>()
                .HasMany(u => u.FavouriteRecipes)
                .WithMany(r => r.UsersFavourite)
                .UsingEntity<Dictionary<string, object>>(
                    "UserFavoriteRecipe", // nazwa tabeli pośredniej
                    j => j.HasOne<Recipe>().WithMany().HasForeignKey("RecipeId"),
                    j => j.HasOne<User>().WithMany().HasForeignKey("UserId")
                );
        }

    }
}
