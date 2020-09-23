using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication4.Migrations
{
    public partial class InitialCreate111 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Pictures_PictureID",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_PictureID",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "PictureID",
                table: "Articles");

            migrationBuilder.AddColumn<int>(
                name: "ArticleID",
                table: "Pictures",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Image",
                table: "Articles",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_ArticleID",
                table: "Pictures",
                column: "ArticleID");

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_Articles_ArticleID",
                table: "Pictures",
                column: "ArticleID",
                principalTable: "Articles",
                principalColumn: "ArticleID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_Articles_ArticleID",
                table: "Pictures");

            migrationBuilder.DropIndex(
                name: "IX_Pictures_ArticleID",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "ArticleID",
                table: "Pictures");

            migrationBuilder.AlterColumn<byte[]>(
                name: "Image",
                table: "Articles",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PictureID",
                table: "Articles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Articles_PictureID",
                table: "Articles",
                column: "PictureID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Pictures_PictureID",
                table: "Articles",
                column: "PictureID",
                principalTable: "Pictures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
