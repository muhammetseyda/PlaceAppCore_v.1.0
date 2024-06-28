using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlaceApp.Migrations
{
    /// <inheritdoc />
    public partial class AddUserNameSharePlace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "SharePlace",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "SharePlace");
        }
    }
}
