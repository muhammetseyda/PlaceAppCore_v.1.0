using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlaceApp.Migrations
{
    /// <inheritdoc />
    public partial class FixSharedList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Places_SharedLists_SharedListsId",
                table: "Places");

            migrationBuilder.DropIndex(
                name: "IX_Places_SharedListsId",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "SharedListsId",
                table: "Places");

            migrationBuilder.CreateTable(
                name: "SharedListPlace",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SharedListId = table.Column<int>(type: "int", nullable: false),
                    PlaceId = table.Column<int>(type: "int", nullable: false)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SharedListPlace");

            migrationBuilder.AddColumn<int>(
                name: "SharedListsId",
                table: "Places",
                type: "int",
                nullable: true);

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
    }
}
