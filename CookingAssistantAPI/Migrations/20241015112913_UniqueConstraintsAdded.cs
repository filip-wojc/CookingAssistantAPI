using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CookingAssistantAPI.Migrations
{
    /// <inheritdoc />
    public partial class UniqueConstraintsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nutrients_NutrientName",
                table: "Nutrients",
                column: "NutrientName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_IngredientName",
                table: "Ingredients",
                column: "IngredientName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserName",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Nutrients_NutrientName",
                table: "Nutrients");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_IngredientName",
                table: "Ingredients");
        }
    }
}
