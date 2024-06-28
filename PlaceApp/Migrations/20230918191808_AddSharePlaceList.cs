using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlaceApp.Migrations
{
    /// <inheritdoc />
    public partial class AddSharePlaceList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SharePlaceListId",
                table: "Places",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SharePlaceList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ListName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ListDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlaceIds = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharePlaceList", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Places_SharePlaceListId",
                table: "Places",
                column: "SharePlaceListId");

            migrationBuilder.AddForeignKey(
                name: "FK_Places_SharePlaceList_SharePlaceListId",
                table: "Places",
                column: "SharePlaceListId",
                principalTable: "SharePlaceList",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Places_SharePlaceList_SharePlaceListId",
                table: "Places");

            migrationBuilder.DropTable(
                name: "SharePlaceList");

            migrationBuilder.DropIndex(
                name: "IX_Places_SharePlaceListId",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "SharePlaceListId",
                table: "Places");
        }
    }
}
