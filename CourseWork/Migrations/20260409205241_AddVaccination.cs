using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseWork.Migrations
{
    /// <inheritdoc />
    public partial class AddVaccination : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vaccinations_animals_animal_id",
                table: "Vaccinations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vaccinations",
                table: "Vaccinations");

            migrationBuilder.RenameTable(
                name: "Vaccinations",
                newName: "vaccinations");

            migrationBuilder.RenameIndex(
                name: "IX_Vaccinations_animal_id",
                table: "vaccinations",
                newName: "IX_vaccinations_animal_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_vaccinations",
                table: "vaccinations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_vaccinations_animals_animal_id",
                table: "vaccinations",
                column: "animal_id",
                principalTable: "animals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_vaccinations_animals_animal_id",
                table: "vaccinations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_vaccinations",
                table: "vaccinations");

            migrationBuilder.RenameTable(
                name: "vaccinations",
                newName: "Vaccinations");

            migrationBuilder.RenameIndex(
                name: "IX_vaccinations_animal_id",
                table: "Vaccinations",
                newName: "IX_Vaccinations_animal_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vaccinations",
                table: "Vaccinations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vaccinations_animals_animal_id",
                table: "Vaccinations",
                column: "animal_id",
                principalTable: "animals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
