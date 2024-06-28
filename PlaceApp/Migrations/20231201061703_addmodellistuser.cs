using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlaceApp.Migrations
{
    /// <inheritdoc />
    public partial class addmodellistuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsersId",
                table: "SharePlaceList",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UsersId",
                table: "SharePlace",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SharePlaceList_UsersId",
                table: "SharePlaceList",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_SharePlace_UsersId",
                table: "SharePlace",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_SharePlace_Users_UsersId",
                table: "SharePlace",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SharePlaceList_Users_UsersId",
                table: "SharePlaceList",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SharePlace_Users_UsersId",
                table: "SharePlace");

            migrationBuilder.DropForeignKey(
                name: "FK_SharePlaceList_Users_UsersId",
                table: "SharePlaceList");

            migrationBuilder.DropIndex(
                name: "IX_SharePlaceList_UsersId",
                table: "SharePlaceList");

            migrationBuilder.DropIndex(
                name: "IX_SharePlace_UsersId",
                table: "SharePlace");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "SharePlaceList");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "SharePlace");
        }
    }
}
