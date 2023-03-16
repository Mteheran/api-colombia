using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class NewFieldsForCountry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string[]>(
                name: "Borders",
                table: "Country",
                type: "text[]",
                nullable: false,
                defaultValue: new string[0]);

            migrationBuilder.AddColumn<string>(
                name: "CurrencySymbol",
                table: "Country",
                type: "character varying(2)",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "Flags",
                table: "Country",
                type: "text[]",
                nullable: false,
                defaultValue: new string[0]);

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Country",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubRegion",
                table: "Country",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Borders",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "CurrencySymbol",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "Flags",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "SubRegion",
                table: "Country");
        }
    }
}
