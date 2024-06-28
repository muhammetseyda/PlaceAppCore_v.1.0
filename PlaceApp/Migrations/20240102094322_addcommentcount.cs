using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlaceApp.Migrations
{
    /// <inheritdoc />
    public partial class addcommentcount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Comment_Users_UsersId",
            //    table: "Comment");

            migrationBuilder.AddColumn<int>(
                name: "CommentCount",
                table: "SharePlaceList",
                type: "int",
                nullable: false,
                defaultValue: 0);

            //migrationBuilder.AlterColumn<int>(
            //    name: "UsersId",
            //    table: "Comment",
            //    type: "int",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "int");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Comment_Users_UsersId",
            //    table: "Comment",
            //    column: "UsersId",
            //    principalTable: "Users",
            //    principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Comment_Users_UsersId",
            //    table: "Comment");

            //migrationBuilder.DropColumn(
            //    name: "CommentCount",
            //    table: "SharePlaceList");

            //migrationBuilder.AlterColumn<int>(
            //    name: "UsersId",
            //    table: "Comment",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0,
            //    oldClrType: typeof(int),
            //    oldType: "int",
            //    oldNullable: true);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Comment_Users_UsersId",
            //    table: "Comment",
            //    column: "UsersId",
            //    principalTable: "Users",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }
    }
}
