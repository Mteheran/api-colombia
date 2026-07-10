using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class RequestMetricRollupTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequestMetricRollup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HourBucketUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RequestCount = table.Column<long>(type: "bigint", nullable: false),
                    TotalResponseBytes = table.Column<long>(type: "bigint", nullable: false),
                    MaxResponseBytes = table.Column<long>(type: "bigint", nullable: false),
                    LargestRequestPath = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestMetricRollup", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestMetricRollup_HourBucketUtc",
                table: "RequestMetricRollup",
                column: "HourBucketUtc",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestMetricRollup");
        }
    }
}
