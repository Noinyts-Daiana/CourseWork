using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseWork.Migrations
{
    /// <inheritdoc />
    public partial class EndAddingAllTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "adopt_animals",
                newName: "arrival_at");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "adopt_animals",
                newName: "adopt_at");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "arrival_at",
                table: "adopt_animals",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "adopt_at",
                table: "adopt_animals",
                newName: "created_at");
        }
    }
}
