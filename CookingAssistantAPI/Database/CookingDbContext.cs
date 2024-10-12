using CookingAssistantAPI.Database.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;

namespace CookingAssistantAPI.Database
{
    public class CookingDbContext : DbContext
    {
        public CookingDbContext(DbContextOptions<CookingDbContext> options) : base(options)
        {
            
        }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Step> Steps { get; set; }
        public DbSet<Nutrient> Nutrients { get; set; }
        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Sqlite:ForeignKeys", true);

            // many to many Recipe --- Ingredient relationship config
            modelBuilder.Entity<RecipeIngredient>() // composite key
                .HasKey(ri => new { ri.RecipeId, ri.IngredientId });

            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(ri => ri.Recipe)
                .WithMany(r => r.RecipeIngredients)
                .HasForeignKey(ri => ri.RecipeId);

            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(ri => ri.Ingredient)
                .WithMany(i => i.RecipeIngredients)
                .HasForeignKey(ri => ri.IngredientId);

            // Many to Many relationship Recipe --- Nutrient
            modelBuilder.Entity<RecipeNutrient>() // composite key
                .HasKey(rn => new { rn.RecipeId, rn.NutrientId });

            modelBuilder.Entity<RecipeNutrient>()
                .HasOne(rn => rn.Recipe)
                .WithMany(r => r.RecipeNutrients)
                .HasForeignKey(rn => rn.RecipeId);

            modelBuilder.Entity<RecipeNutrient>()
                .HasOne(rn => rn.Nutrient)
                .WithMany(n => n.RecipeNutrients)
                .HasForeignKey(rn => rn.NutrientId);
            /*

            // Unique constraints
            modelBuilder.Entity<Ingredient>()
                .HasIndex(i => i.IngredientName)
                .IsUnique();

            modelBuilder.Entity<Nutrient>()
                .HasIndex(n => n.NutrientName)
                .IsUnique();
            */

            // Many to one relationship User <--- Recipe
            modelBuilder.Entity<User>()
                .HasMany(u => u.CreatedRecipes)
                .WithOne(r => r.CreatedBy)
                .HasForeignKey(r => r.CreatedById);

            // Relacja wiele-do-wielu: Użytkownicy --- Ulubione przepisy
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
