using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlaceApp.Migrations
{
    /// <inheritdoc />
    public partial class addplacelistuserpublic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsersId",
                table: "PlaceLists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PlaceLists_UsersId",
                table: "PlaceLists",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlaceLists_Users_UsersId",
                table: "PlaceLists",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlaceLists_Users_UsersId",
                table: "PlaceLists");

            migrationBuilder.DropIndex(
                name: "IX_PlaceLists_UsersId",
                table: "PlaceLists");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "PlaceLists");
        }
    }
}
