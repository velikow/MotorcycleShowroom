using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotorcycleShowroom.Data.Migrations
{
    public partial class secondupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Motorcycles",
                table: "BMW",
                newName: "Motorcycles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Motorcycles",
                table: "BMW",
                newName: "Motorcycles");
        }
    }
}
