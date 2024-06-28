using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlaceApp.Migrations
{
    /// <inheritdoc />
    public partial class addplaceuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsersId",
                table: "Places",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Places_UsersId",
                table: "Places",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Places_Users_UsersId",
                table: "Places",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Places_Users_UsersId",
                table: "Places");

            migrationBuilder.DropIndex(
                name: "IX_Places_UsersId",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "Places");
        }
    }
}
