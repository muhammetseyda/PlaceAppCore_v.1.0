using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlaceApp.Migrations
{
    /// <inheritdoc />
    public partial class AddListIdSharePlaceList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ListId",
                table: "SharePlaceList",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ListId",
                table: "SharePlaceList");
        }
    }
}
