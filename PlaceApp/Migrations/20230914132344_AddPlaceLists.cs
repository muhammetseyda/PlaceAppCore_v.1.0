using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlaceApp.Migrations
{
    /// <inheritdoc />
    public partial class AddPlaceLists : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SharedListPlace");

            migrationBuilder.DropTable(
                name: "SharedLists");

            migrationBuilder.AddColumn<int>(
                name: "PlaceListsId",
                table: "Places",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PlaceLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ListName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ListDescription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaceLists", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Places_PlaceListsId",
                table: "Places",
                column: "PlaceListsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Places_PlaceLists_PlaceListsId",
                table: "Places",
                column: "PlaceListsId",
                principalTable: "PlaceLists",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Places_PlaceLists_PlaceListsId",
                table: "Places");

            migrationBuilder.DropTable(
                name: "PlaceLists");

            migrationBuilder.DropIndex(
                name: "IX_Places_PlaceListsId",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "PlaceListsId",
                table: "Places");

            migrationBuilder.CreateTable(
                name: "SharedLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedLists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SharedListPlace",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlaceId = table.Column<int>(type: "int", nullable: false),
                    SharedListId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedListPlace", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SharedListPlace_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SharedListPlace_SharedLists_SharedListId",
                        column: x => x.SharedListId,
                        principalTable: "SharedLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SharedListPlace_PlaceId",
                table: "SharedListPlace",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedListPlace_SharedListId",
                table: "SharedListPlace",
                column: "SharedListId");
        }
    }
}
