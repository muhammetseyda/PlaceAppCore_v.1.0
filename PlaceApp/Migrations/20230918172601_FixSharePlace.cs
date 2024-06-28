using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlaceApp.Migrations
{
    /// <inheritdoc />
    public partial class FixSharePlace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RetryWent",
                table: "SharePlace");

            migrationBuilder.DropColumn(
                name: "Went",
                table: "SharePlace");

            migrationBuilder.AddColumn<string>(
                name: "UserDescription",
                table: "SharePlace",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserDescription",
                table: "SharePlace");

            migrationBuilder.AddColumn<bool>(
                name: "RetryWent",
                table: "SharePlace",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Went",
                table: "SharePlace",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
