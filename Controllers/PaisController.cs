using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using albumMundial.Models;
using albumMundial.Data;

public class PaisController : Controller
{
    private readonly ApplicationDbContext _context;

    public PaisController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Pais
    public async Task<IActionResult> Index(string? buscar)
    {
        var paises = _context.Paises
     .Include(p => p.Equipo)  
     .AsQueryable();
        if (!string.IsNullOrEmpty(buscar))
            paises = paises.Where(p => p.Nombre.Contains(buscar)
                                   || p.Continente.Contains(buscar));

        ViewBag.Buscar = buscar;
        return View(await paises.ToListAsync());
    }

    // GET: Pais/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var pais = await _context.Paises
            .Include(p => p.Equipo)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (pais == null) return NotFound();

        return View(pais);
    }

    // GET: Pais/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Pais/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nombre,Continente,CodigoFifa,RankingFifa")] Pais pais)
    {
        if (ModelState.IsValid)
        {
            _context.Add(pais);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(pais);
    }

    // GET: Pais/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var pais = await _context.Paises.FindAsync(id);
        if (pais == null) return NotFound();

        return View(pais);
    }

    // POST: Pais/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Nombre,Continente,CodigoFifa,RankingFifa")] Pais pais)
    {
        if (id != pais.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(pais);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaisExists(pais.Id))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(pais);
    }

    // GET: Pais/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var pais = await _context.Paises
            .Include(p => p.Equipo)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (pais == null) return NotFound();

        return View(pais);
    }

    // POST: Pais/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var pais = await _context.Paises.FindAsync(id);
        if (pais != null)
            _context.Paises.Remove(pais);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool PaisExists(int? id)
    {
        return _context.Paises.Any(e => e.Id == id);
    }
}