using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookShop.Books.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Books_ProductId",
                table: "Photos");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Photos",
                newName: "BookId");

            migrationBuilder.RenameIndex(
                name: "IX_Photos_ProductId",
                table: "Photos",
                newName: "IX_Photos_BookId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Books_BookId",
                table: "Photos");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "Photos");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "Photos",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Photos_BookId",
                table: "Photos",
                newName: "IX_Photos_ProductId");

            migrationBuilder.UpdateData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 6, 22, 17, 22, 59, 957, DateTimeKind.Local).AddTicks(1544));

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 6, 22, 17, 22, 59, 961, DateTimeKind.Local).AddTicks(7358));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2021, 6, 22, 17, 22, 59, 961, DateTimeKind.Local).AddTicks(5051));

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Books_ProductId",
                table: "Photos",
                column: "ProductId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
