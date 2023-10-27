using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace api.Migrations
{
    public partial class indiginuosreservation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IndigenousReservation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Code = table.Column<string>(type: "text", nullable: true),
                    ProcedureType = table.Column<string>(type: "text", nullable: true),
                    AdministrativeActType = table.Column<string>(type: "text", nullable: true),
                    AdministrativeActNumber = table.Column<int>(type: "integer", nullable: true),
                    AdministrativeActDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    NativeCommunityId = table.Column<int>(type: "integer", nullable: true),
                    DeparmentId = table.Column<int>(type: "integer", nullable: true),
                    CityId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndigenousReservation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IndigenousReservation_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IndigenousReservation_Department_DeparmentId",
                        column: x => x.DeparmentId,
                        principalTable: "Department",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_IndigenousReservation_NativeCommunity_NativeCommunityId",
                        column: x => x.NativeCommunityId,
                        principalTable: "NativeCommunity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IndigenousReservation_CityId",
                table: "IndigenousReservation",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_IndigenousReservation_DeparmentId",
                table: "IndigenousReservation",
                column: "DeparmentId");

            migrationBuilder.CreateIndex(
                name: "IX_IndigenousReservation_NativeCommunityId",
                table: "IndigenousReservation",
                column: "NativeCommunityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IndigenousReservation");
        }
    }
}
