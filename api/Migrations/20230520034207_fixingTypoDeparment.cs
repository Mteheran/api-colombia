using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class fixingTypoDeparment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Map_Department_DepartamentId",
                table: "Map");

            migrationBuilder.RenameColumn(
                name: "DepartamentId",
                table: "Map",
                newName: "DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Map_DepartamentId",
                table: "Map",
                newName: "IX_Map_DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Map_Department_DepartmentId",
                table: "Map",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Map_Department_DepartmentId",
                table: "Map");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "Map",
                newName: "DepartamentId");

            migrationBuilder.RenameIndex(
                name: "IX_Map_DepartmentId",
                table: "Map",
                newName: "IX_Map_DepartamentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Map_Department_DepartamentId",
                table: "Map",
                column: "DepartamentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
