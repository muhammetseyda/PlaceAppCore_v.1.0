using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlaceApp.Migrations
{
    /// <inheritdoc />
    public partial class sharePlaceaddshowcolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Show",
                table: "SharePlace",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Show",
                table: "SharePlace");
        }
    }
}
