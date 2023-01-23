using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    public partial class UpdateDepartaments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Department_City_CityCapitalId",
                table: "Department");

            migrationBuilder.AlterColumn<int>(
                name: "CityCapitalId",
                table: "Department",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Department_City_CityCapitalId",
                table: "Department",
                column: "CityCapitalId",
                principalTable: "City",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Department_City_CityCapitalId",
                table: "Department");

            migrationBuilder.AlterColumn<int>(
                name: "CityCapitalId",
                table: "Department",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Department_City_CityCapitalId",
                table: "Department",
                column: "CityCapitalId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
