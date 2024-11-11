using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CookingAssistantAPI.Migrations
{
    /// <inheritdoc />
    public partial class CaloricityAndOccasion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeNutrients");

            migrationBuilder.DropTable(
                name: "Nutrients");

            migrationBuilder.AddColumn<int>(
                name: "Caloricity",
                table: "Recipes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Occasion",
                table: "Recipes",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Caloricity",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "Occasion",
                table: "Recipes");

            migrationBuilder.CreateTable(
                name: "Nutrients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NutrientName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nutrients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecipeNutrients",
                columns: table => new
                {
                    RecipeId = table.Column<int>(type: "INTEGER", nullable: false),
                    NutrientId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<string>(type: "TEXT", nullable: true),
                    Unit = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeNutrients", x => new { x.RecipeId, x.NutrientId });
                    table.ForeignKey(
                        name: "FK_RecipeNutrients_Nutrients_NutrientId",
                        column: x => x.NutrientId,
                        principalTable: "Nutrients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeNutrients_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Nutrients_NutrientName",
                table: "Nutrients",
                column: "NutrientName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeNutrients_NutrientId",
                table: "RecipeNutrients",
                column: "NutrientId");
        }
    }
}
