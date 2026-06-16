using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using albumMundial.Models;
using albumMundial.Data;

public class JugadoresController : Controller
{
    private readonly ApplicationDbContext _context;

    public JugadoresController(ApplicationDbContext context)
    {
        _context = context;
    }

    // =========================
    // INDEX
    // =========================
    public async Task<IActionResult> Index(string searchString)
    {
        var jugadores = _context.Jugadores
            .Include(j => j.Equipo)
            .Include(j => j.Cromo)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            jugadores = jugadores.Where(j =>
                j.Nombre.Contains(searchString) ||
                (j.Equipo != null && j.Equipo.Nombre.Contains(searchString)));
        }

        ViewData["CurrentFilter"] = searchString;

        return View(await jugadores.ToListAsync());
    }

    // =========================
    // POR EQUIPO
    // =========================
    public async Task<IActionResult> PorEquipo(int id)
    {
        var jugadores = _context.Jugadores
            .Include(j => j.Equipo)
            .Include(j => j.Cromo)
            .Where(j => j.EquipoId == id);

        ViewData["EquipoId"] = id;

        ViewData["EquipoNombre"] = jugadores.FirstOrDefault()?.Equipo?.Nombre;

        return View(await jugadores.OrderBy(j => j.Nombre).ToListAsync());
    }

    // =========================
    // DETAILS
    // =========================
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var jugador = await _context.Jugadores
            .Include(j => j.Equipo)
            .Include(j => j.Cromo)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (jugador == null) return NotFound();

        return View(jugador);
    }

    // =========================
    // CREATE GET
    // =========================
    public IActionResult Create()
    {
        ViewData["EquipoId"] = new SelectList(
            _context.Equipos.OrderBy(e => e.Nombre),
            "Id",
            "Nombre");

        return View();
    }

    // =========================
    // CREATE POST (CON FOTO)
    // =========================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,Nombre,Posicion,NumeroCamiseta,FechaNacimiento,EquipoId")]
        Jugador jugador,
        IFormFile Foto)
    {
        if (Foto != null && Foto.Length > 0)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(Foto.FileName);

            var path = Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot/images/jugadores", fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await Foto.CopyToAsync(stream);
            }

            jugador.FotoUrl = "/images/jugadores/" + fileName;
        }

        if (ModelState.IsValid)
        {
            _context.Add(jugador);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["EquipoId"] = new SelectList(
            _context.Equipos.OrderBy(e => e.Nombre),
            "Id",
            "Nombre",
            jugador.EquipoId);

        return View(jugador);
    }

    // =========================
    // EDIT GET
    // =========================
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var jugador = await _context.Jugadores.FindAsync(id);

        if (jugador == null) return NotFound();

        ViewData["EquipoId"] = new SelectList(
            _context.Equipos.OrderBy(e => e.Nombre),
            "Id",
            "Nombre",
            jugador.EquipoId);

        return View(jugador);
    }

    // =========================
    // EDIT POST (CON FOTO)
    // =========================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int? id,
        [Bind("Id,Nombre,Posicion,NumeroCamiseta,FechaNacimiento,EquipoId,FotoUrl")]
        Jugador jugador,
        IFormFile Foto)
    {
        if (id != jugador.Id) return NotFound();

        if (Foto != null && Foto.Length > 0)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(Foto.FileName);

            var path = Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot/images/jugadores", fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await Foto.CopyToAsync(stream);
            }

            jugador.FotoUrl = "/images/jugadores/" + fileName;
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(jugador);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JugadorExists(jugador.Id))
                    return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["EquipoId"] = new SelectList(
            _context.Equipos.OrderBy(e => e.Nombre),
            "Id",
            "Nombre",
            jugador.EquipoId);

        return View(jugador);
    }

    // =========================
    // DELETE GET
    // =========================
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var jugador = await _context.Jugadores
            .Include(j => j.Equipo)
            .Include(j => j.Cromo)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (jugador == null) return NotFound();

        return View(jugador);
    }

    // =========================
    // DELETE POST
    // =========================
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var jugador = await _context.Jugadores.FindAsync(id);

        if (jugador != null)
        {
            _context.Jugadores.Remove(jugador);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    // =========================
    // EXISTS
    // =========================
    private bool JugadorExists(int? id)
    {
        return _context.Jugadores.Any(e => e.Id == id);
    }
}