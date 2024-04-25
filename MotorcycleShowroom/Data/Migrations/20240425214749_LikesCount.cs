using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotorcycleShowroom.Data.Migrations
{
    public partial class LikesCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "Likes",
                newName: "BMWId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BMWId",
                table: "Likes",
                newName: "PostId");
        }
    }
}
