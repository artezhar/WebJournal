using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebZhurnal.Data.Migrations
{
    public partial class migr13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rate",
                table: "LogItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Student",
                table: "LogItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Teacher",
                table: "LogItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rate",
                table: "LogItems");

            migrationBuilder.DropColumn(
                name: "Student",
                table: "LogItems");

            migrationBuilder.DropColumn(
                name: "Teacher",
                table: "LogItems");
        }
    }
}
