using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlaceApp.Migrations
{
    /// <inheritdoc />
    public partial class FixComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UsersId",
                table: "Comment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Comment_UsersId",
            //    table: "Comment",
            //    column: "UsersId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Comment_Users_UsersId",
            //    table: "Comment",
            //    column: "UsersId",
            //    principalTable: "Users",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Comment_Users_UsersId",
            //    table: "Comment");

            //migrationBuilder.DropIndex(
            //    name: "IX_Comment_UsersId",
            //    table: "Comment");

            migrationBuilder.DropColumn(
                name: "UsersId",
                table: "Comment");
        }
    }
}
