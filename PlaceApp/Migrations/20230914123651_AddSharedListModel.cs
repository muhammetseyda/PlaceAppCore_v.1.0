using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlaceApp.Migrations
{
    /// <inheritdoc />
    public partial class AddSharedListModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SharedListsId",
                table: "Places",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SharedLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedLists", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Places_SharedListsId",
                table: "Places",
                column: "SharedListsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Places_SharedLists_SharedListsId",
                table: "Places",
                column: "SharedListsId",
                principalTable: "SharedLists",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Places_SharedLists_SharedListsId",
                table: "Places");

            migrationBuilder.DropTable(
                name: "SharedLists");

            migrationBuilder.DropIndex(
                name: "IX_Places_SharedListsId",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "SharedListsId",
                table: "Places");
        }
    }
}
