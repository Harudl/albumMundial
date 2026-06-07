using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace albumMundial.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Albumes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Anio = table.Column<int>(type: "int", nullable: false),
                    CantidadCromos = table.Column<int>(type: "int", nullable: false),
                    EdicionEspecial = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albumes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Paises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Continente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodigoFifa = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    RankingFifa = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paises", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Equipos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DirectorTecnico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnioFundacion = table.Column<int>(type: "int", nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GrupoMundialista = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaisId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Equipos_Paises_PaisId",
                        column: x => x.PaisId,
                        principalTable: "Paises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Jugadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Posicion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroCamiseta = table.Column<int>(type: "int", nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EquipoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jugadores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Jugadores_Equipos_EquipoId",
                        column: x => x.EquipoId,
                        principalTable: "Equipos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cromos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroCromo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Edicion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValorMercado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JugadorId = table.Column<int>(type: "int", nullable: false),
                    AlbumId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cromos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cromos_Albumes_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albumes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cromos_Jugadores_JugadorId",
                        column: x => x.JugadorId,
                        principalTable: "Jugadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cromos_AlbumId",
                table: "Cromos",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_Cromos_JugadorId",
                table: "Cromos",
                column: "JugadorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Equipos_PaisId",
                table: "Equipos",
                column: "PaisId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jugadores_EquipoId",
                table: "Jugadores",
                column: "EquipoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cromos");

            migrationBuilder.DropTable(
                name: "Albumes");

            migrationBuilder.DropTable(
                name: "Jugadores");

            migrationBuilder.DropTable(
                name: "Equipos");

            migrationBuilder.DropTable(
                name: "Paises");
        }
    }
}
