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
        public DbSet<Category> Categories { get; set; } 
        public DbSet<Difficulty> Difficulties { get; set; } 
        public DbSet<Occasion> Occasions { get; set; } 
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; } 
        public DbSet<User> Users { get; set; }
        public DbSet<Review> Reviews { get; set; }
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


            // Unique constraints
            modelBuilder.Entity<Ingredient>()
                .HasIndex(i => i.IngredientName)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Many to one relationship User <--- Recipe
            modelBuilder.Entity<User>()
                .HasMany(u => u.CreatedRecipes)
                .WithOne(r => r.CreatedBy)
                .HasForeignKey(r => r.CreatedById).OnDelete(DeleteBehavior.Cascade);

            // Many to one relationship User <--- Review
            modelBuilder.Entity<User>()
                .HasMany(u => u.Reviews)
                .WithOne(r => r.ReviewAuthor)
                .HasForeignKey(r => r.ReviewAuthorId).OnDelete(DeleteBehavior.Cascade);

            // Relacja wiele-do-wielu: Użytkownicy --- Ulubione przepisy
            modelBuilder.Entity<User>()
                .HasMany(u => u.FavouriteRecipes)
                .WithMany(r => r.UsersFavourite)
                .UsingEntity<Dictionary<string, object>>(
                    "UserFavoriteRecipe", // nazwa tabeli pośredniej
                    j => j.HasOne<Recipe>().WithMany().HasForeignKey("RecipeId").OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<User>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.Cascade)
                );
        }

    }
}
