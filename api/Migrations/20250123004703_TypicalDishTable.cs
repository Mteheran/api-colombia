using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class TypicalDishTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Department_Region_RegionId",
                table: "Department");

            migrationBuilder.DropForeignKey(
                name: "FK_IndigenousReservation_City_CityId",
                table: "IndigenousReservation");

            migrationBuilder.DropForeignKey(
                name: "FK_IndigenousReservation_NativeCommunity_NativeCommunityId",
                table: "IndigenousReservation");

            migrationBuilder.DropForeignKey(
                name: "FK_Map_Department_DepartmentId",
                table: "Map");

            migrationBuilder.DropForeignKey(
                name: "FK_NaturalArea_Department_DepartmentId",
                table: "NaturalArea");

            migrationBuilder.CreateTable(
                name: "Holidays",
                columns: table => new
                {
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holidays", x => x.Date);
                });

            migrationBuilder.CreateTable(
                name: "TypicalDish",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Ingredients = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    DepartmentId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypicalDish", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TypicalDish_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TypicalDish_DepartmentId",
                table: "TypicalDish",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Department_Region_RegionId",
                table: "Department",
                column: "RegionId",
                principalTable: "Region",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IndigenousReservation_City_CityId",
                table: "IndigenousReservation",
                column: "CityId",
                principalTable: "City",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IndigenousReservation_NativeCommunity_NativeCommunityId",
                table: "IndigenousReservation",
                column: "NativeCommunityId",
                principalTable: "NativeCommunity",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Map_Department_DepartmentId",
                table: "Map",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_NaturalArea_Department_DepartmentId",
                table: "NaturalArea",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Department_Region_RegionId",
                table: "Department");

            migrationBuilder.DropForeignKey(
                name: "FK_IndigenousReservation_City_CityId",
                table: "IndigenousReservation");

            migrationBuilder.DropForeignKey(
                name: "FK_IndigenousReservation_NativeCommunity_NativeCommunityId",
                table: "IndigenousReservation");

            migrationBuilder.DropForeignKey(
                name: "FK_Map_Department_DepartmentId",
                table: "Map");

            migrationBuilder.DropForeignKey(
                name: "FK_NaturalArea_Department_DepartmentId",
                table: "NaturalArea");

            migrationBuilder.DropTable(
                name: "Holidays");

            migrationBuilder.DropTable(
                name: "TypicalDish");

            migrationBuilder.AddForeignKey(
                name: "FK_Department_Region_RegionId",
                table: "Department",
                column: "RegionId",
                principalTable: "Region",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IndigenousReservation_City_CityId",
                table: "IndigenousReservation",
                column: "CityId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IndigenousReservation_NativeCommunity_NativeCommunityId",
                table: "IndigenousReservation",
                column: "NativeCommunityId",
                principalTable: "NativeCommunity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Map_Department_DepartmentId",
                table: "Map",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NaturalArea_Department_DepartmentId",
                table: "NaturalArea",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
