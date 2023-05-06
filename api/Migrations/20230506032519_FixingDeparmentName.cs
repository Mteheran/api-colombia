using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class FixingDeparmentName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_City_Department_DepartamentId",
                table: "City");

            migrationBuilder.DropForeignKey(
                name: "FK_Department_City_CityCapitalId",
                table: "Department");

            migrationBuilder.DropIndex(
                name: "IX_Department_CityCapitalId",
                table: "Department");

            migrationBuilder.RenameColumn(
                name: "DepartamentId",
                table: "City",
                newName: "DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_City_DepartamentId",
                table: "City",
                newName: "IX_City_DepartmentId");

            migrationBuilder.AddColumn<int>(
                name: "CityCapitalId1",
                table: "Department",
                type: "integer",
                nullable: false,
                defaultValue: 0);


            migrationBuilder.AddForeignKey(
                name: "FK_City_Department_DepartmentId",
                table: "City",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_City_Department_DepartmentId",
                table: "City");

            migrationBuilder.DropForeignKey(
                name: "FK_Department_City_CityCapitalId1",
                table: "Department");

            migrationBuilder.DropIndex(
                name: "IX_Department_CityCapitalId1",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "CityCapitalId1",
                table: "Department");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "City",
                newName: "DepartamentId");

            migrationBuilder.RenameIndex(
                name: "IX_City_DepartmentId",
                table: "City",
                newName: "IX_City_DepartamentId");

            migrationBuilder.CreateIndex(
                name: "IX_Department_CityCapitalId",
                table: "Department",
                column: "CityCapitalId");

            migrationBuilder.AddForeignKey(
                name: "FK_City_Department_DepartamentId",
                table: "City",
                column: "DepartamentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
