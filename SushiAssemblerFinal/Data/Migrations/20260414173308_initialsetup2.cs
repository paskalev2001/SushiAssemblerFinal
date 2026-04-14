using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SushiAssemblerFinal.Data.Migrations
{
    /// <inheritdoc />
    public partial class initialsetup2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Food",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Food");
        }
    }
}
