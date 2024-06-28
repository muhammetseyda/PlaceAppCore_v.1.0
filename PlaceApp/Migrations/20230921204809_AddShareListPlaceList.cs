using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlaceApp.Migrations
{
    /// <inheritdoc />
    public partial class AddShareListPlaceList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SharePlaceListId",
                table: "SharePlace",
                type: "int",
                nullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_SharePlace_SharePlaceListId",
            //    table: "SharePlace",
            //    column: "SharePlaceListId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_SharePlace_SharePlaceList_SharePlaceListId",
            //    table: "SharePlace",
            //    column: "SharePlaceListId",
            //    principalTable: "SharePlaceList",
            //    principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_SharePlace_SharePlaceList_SharePlaceListId",
            //    table: "SharePlace");

            //migrationBuilder.DropIndex(
            //    name: "IX_SharePlace_SharePlaceListId",
            //    table: "SharePlace");

            migrationBuilder.DropColumn(
                name: "SharePlaceListId",
                table: "SharePlace");
        }
    }
}
