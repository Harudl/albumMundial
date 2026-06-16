using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace albumMundial.Migrations
{
    /// <inheritdoc />
    public partial class fotourl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FotoUrl",
                table: "Jugadores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FotoUrl",
                table: "Albumes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FotoUrl",
                table: "Jugadores");

            migrationBuilder.DropColumn(
                name: "FotoUrl",
                table: "Albumes");
        }
    }
}
