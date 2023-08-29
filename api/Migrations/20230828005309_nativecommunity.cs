using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace api.Migrations
{
    public partial class nativecommunity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NativeCommunity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Languages = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Images = table.Column<string[]>(type: "text[]", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NativeCommunity", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NativeCommunity");
        }
    }
}
