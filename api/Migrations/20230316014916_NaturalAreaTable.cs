using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace api.Migrations
{
    public partial class NaturalAreaTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NaturalArea",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AreaGroupId = table.Column<int>(type: "integer", nullable: false),
                    CategoryNaturalAreaId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    DepartmentId = table.Column<int>(type: "integer", nullable: false),
                    DaneCode = table.Column<int>(type: "integer", nullable: false),
                    LandArea = table.Column<double>(type: "double precision", nullable: false),
                    MaritimeArea = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NaturalArea", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NaturalArea_CategoryNaturalArea_CategoryNaturalAreaId",
                        column: x => x.CategoryNaturalAreaId,
                        principalTable: "CategoryNaturalArea",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NaturalArea_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NaturalArea_CategoryNaturalAreaId",
                table: "NaturalArea",
                column: "CategoryNaturalAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_NaturalArea_DepartmentId",
                table: "NaturalArea",
                column: "DepartmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NaturalArea");
        }
    }
}
