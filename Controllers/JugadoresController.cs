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

    // GET: JUGADORES
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

    // GET: JUGADORES/POR EQUIPO
    public async Task<IActionResult> PorEquipo(int id)
    {
        var jugadores = _context.Jugadores
            .Include(j => j.Equipo)
            .Include(j => j.Cromo)
            .Where(j => j.EquipoId == id);

        ViewData["EquipoId"] = id;

        ViewData["EquipoNombre"] = jugadores
            .FirstOrDefault()?.Equipo?.Nombre;

        return View(await jugadores
            .OrderBy(j => j.Nombre)
            .ToListAsync());
    }

    // GET: JUGADORES/DETAILS/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var jugador = await _context.Jugadores
            .Include(j => j.Equipo)
            .Include(j => j.Cromo)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (jugador == null)
        {
            return NotFound();
        }

        return View(jugador);
    }

    // GET: JUGADORES/CREATE
    public IActionResult Create()
    {
        ViewData["EquipoId"] = new SelectList(
            _context.Equipos.OrderBy(e => e.Nombre),
            "Id",
            "Nombre");

        return View();
    }

    // POST: JUGADORES/CREATE
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,Nombre,Posicion,NumeroCamiseta,FechaNacimiento,EquipoId")]
        Jugador jugador)
    {
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

    // GET: JUGADORES/EDIT/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var jugador = await _context.Jugadores.FindAsync(id);

        if (jugador == null)
        {
            return NotFound();
        }

        ViewData["EquipoId"] = new SelectList(
            _context.Equipos.OrderBy(e => e.Nombre),
            "Id",
            "Nombre",
            jugador.EquipoId);

        return View(jugador);
    }

    // POST: JUGADORES/EDIT/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int? id,
        [Bind("Id,Nombre,Posicion,NumeroCamiseta,FechaNacimiento,EquipoId")]
        Jugador jugador)
    {
        if (id != jugador.Id)
        {
            return NotFound();
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
                {
                    return NotFound();
                }

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

    // GET: JUGADORES/DELETE/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var jugador = await _context.Jugadores
            .Include(j => j.Equipo)
            .Include(j => j.Cromo)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (jugador == null)
        {
            return NotFound();
        }

        return View(jugador);
    }

    // POST: JUGADORES/DELETE/5
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

    private bool JugadorExists(int? id)
    {
        return _context.Jugadores.Any(e => e.Id == id);
    }
}