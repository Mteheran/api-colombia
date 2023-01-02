using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    StateCapital = table.Column<string>(type: "text", nullable: true),
                    Surface = table.Column<float>(type: "real", nullable: false),
                    Population = table.Column<float>(type: "real", nullable: false),
                    Languages = table.Column<string[]>(type: "text[]", nullable: false),
                    TimeZone = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    ISOCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    InternetDomain = table.Column<string>(type: "text", nullable: true),
                    PhonePrefix = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    RadioPrefix = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    AircraftPrefix = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Surface = table.Column<float>(type: "real", nullable: false),
                    Population = table.Column<float>(type: "real", nullable: false),
                    PostalCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    PhonePrefix = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    DepartamentId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CityCapitalId = table.Column<int>(type: "integer", nullable: false),
                    Municipalities = table.Column<string[]>(type: "text[]", nullable: true),
                    Surface = table.Column<float>(type: "real", nullable: false),
                    Population = table.Column<float>(type: "real", nullable: true),
                    PhonePrefix = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    CountryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Department_City_CityCapitalId",
                        column: x => x.CityCapitalId,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Department_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "President",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Image = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    LastName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    StartPeriodDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndPeriodDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PoliticalParty = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CityId = table.Column<int>(type: "integer", nullable: false),
                    CountryId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_President", x => x.Id);
                    table.ForeignKey(
                        name: "FK_President_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_President_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TouristAttraction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Images = table.Column<string[]>(type: "text[]", nullable: true),
                    Latitude = table.Column<string>(type: "text", nullable: true),
                    Longitude = table.Column<string>(type: "text", nullable: true),
                    CityId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TouristAttraction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TouristAttraction_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_City_DepartamentId",
                table: "City",
                column: "DepartamentId");

            migrationBuilder.CreateIndex(
                name: "IX_Department_CityCapitalId",
                table: "Department",
                column: "CityCapitalId");

            migrationBuilder.CreateIndex(
                name: "IX_Department_CountryId",
                table: "Department",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_President_CityId",
                table: "President",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_President_CountryId",
                table: "President",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_TouristAttraction_CityId",
                table: "TouristAttraction",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_City_Department_DepartamentId",
                table: "City",
                column: "DepartamentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_City_Department_DepartamentId",
                table: "City");

            migrationBuilder.DropTable(
                name: "President");

            migrationBuilder.DropTable(
                name: "TouristAttraction");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "Country");
        }
    }
}
