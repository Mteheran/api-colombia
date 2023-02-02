using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class UpdateCityDepartments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Municipalities",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "PhonePrefix",
                table: "City");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string[]>(
                name: "Municipalities",
                table: "Department",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhonePrefix",
                table: "City",
                type: "character varying(5)",
                maxLength: 5,
                nullable: true);
        }
    }
}
