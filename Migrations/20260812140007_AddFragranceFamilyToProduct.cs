using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace project_cuoiky.Migrations
{
    /// <inheritdoc />
    public partial class AddFragranceFamilyToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FragranceFamily",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FragranceFamily",
                table: "Products");
        }
    }
}
