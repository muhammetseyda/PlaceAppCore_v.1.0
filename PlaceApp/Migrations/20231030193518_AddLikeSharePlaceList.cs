﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlaceApp.Migrations
{
    /// <inheritdoc />
    public partial class AddLikeSharePlaceList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LikeCount",
                table: "SharePlaceList",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LikeCount",
                table: "SharePlaceList");
        }
    }
}
