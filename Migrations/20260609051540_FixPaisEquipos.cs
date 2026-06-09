using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace albumMundial.Migrations
{
    /// <inheritdoc />
    public partial class FixPaisEquipos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Equipos_PaisId",
                table: "Equipos");

            migrationBuilder.CreateIndex(
                name: "IX_Equipos_PaisId",
                table: "Equipos",
                column: "PaisId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Equipos_PaisId",
                table: "Equipos");

            migrationBuilder.CreateIndex(
                name: "IX_Equipos_PaisId",
                table: "Equipos",
                column: "PaisId",
                unique: true);
        }
    }
}
