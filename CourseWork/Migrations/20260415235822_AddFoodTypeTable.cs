using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CourseWork.Migrations
{
    /// <inheritdoc />
    public partial class AddFoodTypeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "animal_photos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    animal_id = table.Column<int>(type: "integer", nullable: false),
                    file_path = table.Column<string>(type: "text", nullable: false),
                    is_main = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_animal_photos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_animal_photos_animals_animal_id",
                        column: x => x.animal_id,
                        principalTable: "animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "food_types",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    brand = table.Column<string>(type: "text", nullable: false),
                    unit = table.Column<string>(type: "text", nullable: false),
                    stock_quantity = table.Column<decimal>(type: "numeric", nullable: false),
                    min_threshold = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_food_types", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "fooding_logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    animal_id = table.Column<int>(type: "integer", nullable: false),
                    food_type_id = table.Column<int>(type: "integer", nullable: false),
                    fed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    fed_by_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fooding_logs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_fooding_logs_animals_animal_id",
                        column: x => x.animal_id,
                        principalTable: "animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_fooding_logs_food_types_food_type_id",
                        column: x => x.food_type_id,
                        principalTable: "food_types",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_fooding_logs_users_fed_by_id",
                        column: x => x.fed_by_id,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_animal_photos_animal_id",
                table: "animal_photos",
                column: "animal_id");

            migrationBuilder.CreateIndex(
                name: "IX_fooding_logs_animal_id",
                table: "fooding_logs",
                column: "animal_id");

            migrationBuilder.CreateIndex(
                name: "IX_fooding_logs_fed_by_id",
                table: "fooding_logs",
                column: "fed_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_fooding_logs_food_type_id",
                table: "fooding_logs",
                column: "food_type_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "animal_photos");

            migrationBuilder.DropTable(
                name: "fooding_logs");

            migrationBuilder.DropTable(
                name: "food_types");
        }
    }
}
