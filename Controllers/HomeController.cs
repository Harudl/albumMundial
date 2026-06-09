using albumMundial.Data;
using albumMundial.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace albumMundial.Controllers
{
    // ViewModel para transportar las estadísticas y colecciones a la vista
    public class HomeDashboardViewModel
    {
        public int TotalCromos { get; set; }
        public decimal TotalValorMercado { get; set; }
        public int TotalPaises { get; set; }
        public int TotalJugadores { get; set; }
        public List<Cromo> UltimosCromos { get; set; } = new List<Cromo>();
    }

    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Inyectamos el contexto de base de datos
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomeDashboardViewModel();

            // 1. Contamos el total de cromos en la colección
            model.TotalCromos = await _context.Cromos.CountAsync();

            // 2. Sumamos el valor total del mercado (si no hay cromos, devuelve 0)
            model.TotalValorMercado = await _context.Cromos.SumAsync(c => c.ValorMercado);

            // 3. Contamos cuántos países únicos participan indirectamente en las relaciones
            model.TotalPaises = await _context.Paises.CountAsync();

            // 4. Cantidad total de jugadores disponibles en el catálogo maestro
            model.TotalJugadores = await _context.Jugadores.CountAsync();

            // 5. Obtenemos los últimos 4 cromos agregados incluyendo la data de su Jugador (para Nombre y Foto)
            model.UltimosCromos = await _context.Cromos
                .Include(c => c.Jugador)
                .Include(c => c.Album)
                .OrderByDescending(c => c.Id) // Suponiendo que el Id es secuencial/autoincremental
                .Take(4)
                .ToListAsync();

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}