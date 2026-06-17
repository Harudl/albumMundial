using albumMundial.Data;
using albumMundial.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;

public class EquipoController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _env;

    public EquipoController(ApplicationDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    // GET: Equipo
    public async Task<IActionResult> Index(string? buscar)
    {
        var equipos = _context.Equipos
            .Include(e => e.Pais)
            .AsQueryable();

        if (!string.IsNullOrEmpty(buscar))
            equipos = equipos.Where(e => e.Nombre.Contains(buscar)
                                     || e.Pais.Nombre.Contains(buscar));

        ViewBag.Buscar = buscar;
        return View(await equipos.ToListAsync());
    }

    // GET: Equipo/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var equipo = await _context.Equipos
            .Include(e => e.Pais)
            .Include(e => e.Jugadores)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (equipo == null) return NotFound();

        return View(equipo);
    }

    // GET: Equipo/Create
    public IActionResult Create()
    {
        ViewBag.PaisId = new SelectList(_context.Paises, "Id", "Nombre");
        return View();
    }

    // POST: Equipo/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Equipo equipo, IFormFile? logoFile)
    {
        ModelState.Remove("Pais");
        ModelState.Remove("Jugadores");
        ModelState.Remove("LogoUrl");

        if (ModelState.IsValid)
        {
            // Verificar si ya existe un equipo con ese nombre
            if (await _context.Equipos.AnyAsync(e => e.Nombre == equipo.Nombre))
            {
                ModelState.AddModelError("Nombre", "Ya existe un equipo con ese nombre.");
                ViewBag.PaisId = new SelectList(_context.Paises, "Id", "Nombre", equipo.PaisId);
                return View(equipo);
            }

            if (logoFile != null && logoFile.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(logoFile.FileName);
                var path = Path.Combine(_env.WebRootPath, "images", fileName);
                using var stream = new FileStream(path, FileMode.Create);
                await logoFile.CopyToAsync(stream);
                equipo.LogoUrl = "/images/" + fileName;
            }

            try
            {
                _context.Add(equipo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("Nombre", "Ya existe un equipo con ese nombre.");
            }
        }

        ViewBag.PaisId = new SelectList(_context.Paises, "Id", "Nombre", equipo.PaisId);
        return View(equipo);
    }

    // GET: Equipo/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var equipo = await _context.Equipos.FindAsync(id);
        if (equipo == null) return NotFound();

        ViewBag.PaisId = new SelectList(_context.Paises, "Id", "Nombre", equipo.PaisId);
        return View(equipo);
    }

    // POST: Equipo/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Equipo equipo, IFormFile? logoFile)
    {
        if (id != equipo.Id) return NotFound();

        ModelState.Remove("Pais");
        ModelState.Remove("Jugadores");
        ModelState.Remove("LogoUrl");

        if (ModelState.IsValid)
        {
            if (logoFile != null && logoFile.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(logoFile.FileName);
                var path = Path.Combine(_env.WebRootPath, "images", fileName);
                using var stream = new FileStream(path, FileMode.Create);
                await logoFile.CopyToAsync(stream);
                equipo.LogoUrl = "/images/" + fileName;
            }
            else
            {
                // Conservar el logo anterior si no se sube uno nuevo
                var existing = await _context.Equipos
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.Id == equipo.Id);
                equipo.LogoUrl = existing?.LogoUrl;
            }

            _context.Update(equipo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewBag.PaisId = new SelectList(_context.Paises, "Id", "Nombre", equipo.PaisId);
        return View(equipo);
    }

    // GET: Equipo/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var equipo = await _context.Equipos
            .Include(e => e.Pais)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (equipo == null) return NotFound();

        return View(equipo);
    }

    // POST: Equipo/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var equipo = await _context.Equipos.FindAsync(id);
        if (equipo != null)
            _context.Equipos.Remove(equipo);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool EquipoExists(int id)
    {
        return _context.Equipos.Any(e => e.Id == id);
    }
}