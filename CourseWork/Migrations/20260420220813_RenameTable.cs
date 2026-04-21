using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseWork.Migrations
{
    /// <inheritdoc />
    public partial class RenameTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_fooding_logs_animals_animal_id",
                table: "fooding_logs");

            migrationBuilder.DropForeignKey(
                name: "FK_fooding_logs_food_types_food_type_id",
                table: "fooding_logs");

            migrationBuilder.DropForeignKey(
                name: "FK_fooding_logs_users_fed_by_id",
                table: "fooding_logs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_fooding_logs",
                table: "fooding_logs");

            migrationBuilder.RenameTable(
                name: "fooding_logs",
                newName: "feeding_logs");

            migrationBuilder.RenameIndex(
                name: "IX_fooding_logs_food_type_id",
                table: "feeding_logs",
                newName: "IX_feeding_logs_food_type_id");

            migrationBuilder.RenameIndex(
                name: "IX_fooding_logs_fed_by_id",
                table: "feeding_logs",
                newName: "IX_feeding_logs_fed_by_id");

            migrationBuilder.RenameIndex(
                name: "IX_fooding_logs_animal_id",
                table: "feeding_logs",
                newName: "IX_feeding_logs_animal_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_feeding_logs",
                table: "feeding_logs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_feeding_logs_animals_animal_id",
                table: "feeding_logs",
                column: "animal_id",
                principalTable: "animals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_feeding_logs_food_types_food_type_id",
                table: "feeding_logs",
                column: "food_type_id",
                principalTable: "food_types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_feeding_logs_users_fed_by_id",
                table: "feeding_logs",
                column: "fed_by_id",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_feeding_logs_animals_animal_id",
                table: "feeding_logs");

            migrationBuilder.DropForeignKey(
                name: "FK_feeding_logs_food_types_food_type_id",
                table: "feeding_logs");

            migrationBuilder.DropForeignKey(
                name: "FK_feeding_logs_users_fed_by_id",
                table: "feeding_logs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_feeding_logs",
                table: "feeding_logs");

            migrationBuilder.RenameTable(
                name: "feeding_logs",
                newName: "fooding_logs");

            migrationBuilder.RenameIndex(
                name: "IX_feeding_logs_food_type_id",
                table: "fooding_logs",
                newName: "IX_fooding_logs_food_type_id");

            migrationBuilder.RenameIndex(
                name: "IX_feeding_logs_fed_by_id",
                table: "fooding_logs",
                newName: "IX_fooding_logs_fed_by_id");

            migrationBuilder.RenameIndex(
                name: "IX_feeding_logs_animal_id",
                table: "fooding_logs",
                newName: "IX_fooding_logs_animal_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_fooding_logs",
                table: "fooding_logs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_fooding_logs_animals_animal_id",
                table: "fooding_logs",
                column: "animal_id",
                principalTable: "animals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_fooding_logs_food_types_food_type_id",
                table: "fooding_logs",
                column: "food_type_id",
                principalTable: "food_types",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_fooding_logs_users_fed_by_id",
                table: "fooding_logs",
                column: "fed_by_id",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
