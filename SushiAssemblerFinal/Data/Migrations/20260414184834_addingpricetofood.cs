using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SushiAssemblerFinal.Data.Migrations
{
    /// <inheritdoc />
    public partial class addingpricetofood : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Food",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Food");
        }
    }
}
