using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebZhurnal.Data.Migrations
{
    public partial class migr3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rates_AspNetUsers_StudentId1",
                table: "Rates");

            migrationBuilder.DropForeignKey(
                name: "FK_Rates_AspNetUsers_TeacherId1",
                table: "Rates");

            migrationBuilder.DropIndex(
                name: "IX_Rates_StudentId1",
                table: "Rates");

            migrationBuilder.DropIndex(
                name: "IX_Rates_TeacherId1",
                table: "Rates");

            migrationBuilder.DropColumn(
                name: "StudentId1",
                table: "Rates");

            migrationBuilder.DropColumn(
                name: "TeacherId1",
                table: "Rates");

            migrationBuilder.AlterColumn<string>(
                name: "TeacherId",
                table: "Rates",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "StudentId",
                table: "Rates",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Rates_StudentId",
                table: "Rates",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Rates_TeacherId",
                table: "Rates",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rates_AspNetUsers_StudentId",
                table: "Rates",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rates_AspNetUsers_TeacherId",
                table: "Rates",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rates_AspNetUsers_StudentId",
                table: "Rates");

            migrationBuilder.DropForeignKey(
                name: "FK_Rates_AspNetUsers_TeacherId",
                table: "Rates");

            migrationBuilder.DropIndex(
                name: "IX_Rates_StudentId",
                table: "Rates");

            migrationBuilder.DropIndex(
                name: "IX_Rates_TeacherId",
                table: "Rates");

            migrationBuilder.AlterColumn<int>(
                name: "TeacherId",
                table: "Rates",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StudentId",
                table: "Rates",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentId1",
                table: "Rates",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TeacherId1",
                table: "Rates",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rates_StudentId1",
                table: "Rates",
                column: "StudentId1");

            migrationBuilder.CreateIndex(
                name: "IX_Rates_TeacherId1",
                table: "Rates",
                column: "TeacherId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Rates_AspNetUsers_StudentId1",
                table: "Rates",
                column: "StudentId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rates_AspNetUsers_TeacherId1",
                table: "Rates",
                column: "TeacherId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
