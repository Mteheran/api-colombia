using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace api.Migrations
{
    public partial class InvasiveSpecie : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvasiveSpecie",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    ScientificName = table.Column<string>(type: "text", nullable: true),
                    CommonNames = table.Column<string>(type: "text", nullable: true),
                    Impact = table.Column<string>(type: "text", nullable: false),
                    Manage = table.Column<string>(type: "text", nullable: true),
                    RiskLevel = table.Column<int>(type: "integer", nullable: false),
                    UrlImage = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvasiveSpecie", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvasiveSpecie");
        }
    }
}
