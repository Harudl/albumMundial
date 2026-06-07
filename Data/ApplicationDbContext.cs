using Microsoft.EntityFrameworkCore;
using albumMundial.Models;

namespace albumMundial.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        //Definicion de las tablas de la Base de Datos
        public DbSet<Pais> Paises { get; set; }
        public DbSet<Equipo> Equipos { get; set; }
        public DbSet<Jugador> Jugadores { get; set; }
        public DbSet<Cromo> Cromos { get; set; }
        public DbSet<Album> Albumes { get; set; }
    }
}
