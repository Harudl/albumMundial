
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using albumMundial.Models;
using albumMundial.Data;

public class AlbumsController : Controller
{
    private readonly ApplicationDbContext _context;

    public AlbumsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: ALBUMS
    public async Task<IActionResult> Index(bool? soloEspecial)    
    {
        var albums = _context.Albumes
            .Include(a => a.Cromos)
            .AsQueryable();

        if (soloEspecial == true)
        {
            albums = albums.Where(a => a.EdicionEspecial);
        }

            ViewData["TotalCromos"] = albums.Sum(a => a.Cromos.Count);

        return View(await albums.ToListAsync());
    }



    // GET: ALBUMS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var album = await _context.Albumes
            .FirstOrDefaultAsync(m => m.Id == id);
        if (album == null)
        {
            return NotFound();
        }

        return View(album);
    }

    // GET: ALBUMS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: ALBUMS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nombre,Anio,CantidadCromos,EdicionEspecial,Cromos")] Album album)
    {
        if (ModelState.IsValid)
        {
            _context.Add(album);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(album);
    }

    // GET: ALBUMS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var album = await _context.Albumes.FindAsync(id);
        if (album == null)
        {
            return NotFound();
        }
        return View(album);
    }

    // POST: ALBUMS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Nombre,Anio,CantidadCromos,EdicionEspecial,Cromos")] Album album)
    {
        if (id != album.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(album);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlbumExists(album.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(album);
    }

    // GET: ALBUMS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var album = await _context.Albumes
            .FirstOrDefaultAsync(m => m.Id == id);
        if (album == null)
        {
            return NotFound();
        }

        return View(album);
    }

    // POST: ALBUMS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var album = await _context.Albumes.FindAsync(id);
        if (album != null)
        {
            _context.Albumes.Remove(album);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool AlbumExists(int? id)
    {
        return _context.Albumes.Any(e => e.Id == id);
    }
}
