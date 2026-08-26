using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace project_cuoiky.Migrations
{
    /// <inheritdoc />
    public partial class AddPerfumeDetailsToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Concentration",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Longevity",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RecommendedTime",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Sillage",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Volume",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Concentration",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Longevity",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "RecommendedTime",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Sillage",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Volume",
                table: "Products");
        }
    }
}
