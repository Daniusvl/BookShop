using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookShop.Books.Migrations
{
    public partial class third : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Books_BookId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "Photos");

            migrationBuilder.AlterColumn<int>(
                name: "BookId",
                table: "Photos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 6, 22, 18, 25, 38, 899, DateTimeKind.Local).AddTicks(3703));

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 6, 22, 18, 25, 38, 903, DateTimeKind.Local).AddTicks(8729));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 6, 22, 18, 25, 38, 903, DateTimeKind.Local).AddTicks(6418));

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Books_BookId",
                table: "Photos",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Books_BookId",
                table: "Photos");

            migrationBuilder.AlterColumn<int>(
                name: "BookId",
                table: "Photos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PhotoId",
                table: "Photos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 6, 22, 18, 15, 9, 13, DateTimeKind.Local).AddTicks(6679));

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 6, 22, 18, 15, 9, 19, DateTimeKind.Local).AddTicks(1162));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 6, 22, 18, 15, 9, 18, DateTimeKind.Local).AddTicks(8649));

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Books_BookId",
                table: "Photos",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
