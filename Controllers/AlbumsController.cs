using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using albumMundial.Models;
using albumMundial.Data;

public class AlbumsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public AlbumsController(
        ApplicationDbContext context,
        IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    // GET: Albums
    public async Task<IActionResult> Index(bool? soloEspeciales)
    {
        var albums = _context.Albumes
            .Include(a => a.Cromos)
            .AsQueryable();

        if (soloEspeciales == true)
        {
            albums = albums.Where(a => a.EdicionEspecial);
        }

        ViewData["TotalCromos"] = albums.Sum(a => a.Cromos.Count);

        return View(await albums.ToListAsync());
    }

    // GET: Albums/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var album = await _context.Albumes
            .Include(a => a.Cromos)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (album == null)
        {
            return NotFound();
        }

        return View(album);
    }

    // GET: Albums/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Albums/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,Nombre,Anio,CantidadCromos,EdicionEspecial,FotoUrl")]
        Album album,
        IFormFile? imagenAlbum)
    {
        if (ModelState.IsValid)
        {
            if (imagenAlbum != null && imagenAlbum.Length > 0)
            {
                string carpetaDestino = Path.Combine(
                    _webHostEnvironment.WebRootPath,
                    "imagenes",
                    "albums");

                if (!Directory.Exists(carpetaDestino))
                {
                    Directory.CreateDirectory(carpetaDestino);
                }

                string nombreArchivo =
                    Guid.NewGuid().ToString() +
                    Path.GetExtension(imagenAlbum.FileName);

                string rutaCompleta =
                    Path.Combine(carpetaDestino, nombreArchivo);

                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    await imagenAlbum.CopyToAsync(stream);
                }

                album.FotoUrl = "/imagenes/albums/" + nombreArchivo;
            }

            _context.Add(album);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        return View(album);
    }

    // GET: Albums/Edit/5
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

    // POST: Albums/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,Nombre,Anio,CantidadCromos,EdicionEspecial,FotoUrl")]
        Album album,
        IFormFile? nuevaImagenAlbum)
    {
        if (id != album.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                if (nuevaImagenAlbum != null && nuevaImagenAlbum.Length > 0)
                {
                    if (!string.IsNullOrEmpty(album.FotoUrl))
                    {
                        string imagenAnterior = Path.Combine(
                            _webHostEnvironment.WebRootPath,
                            album.FotoUrl.TrimStart('/'));

                        if (System.IO.File.Exists(imagenAnterior))
                        {
                            System.IO.File.Delete(imagenAnterior);
                        }
                    }

                    string carpetaDestino = Path.Combine(
                        _webHostEnvironment.WebRootPath,
                        "imagenes",
                        "albums");

                    if (!Directory.Exists(carpetaDestino))
                    {
                        Directory.CreateDirectory(carpetaDestino);
                    }

                    string nombreArchivo =
                        Guid.NewGuid().ToString() +
                        Path.GetExtension(nuevaImagenAlbum.FileName);

                    string rutaCompleta =
                        Path.Combine(carpetaDestino, nombreArchivo);

                    using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                    {
                        await nuevaImagenAlbum.CopyToAsync(stream);
                    }

                    album.FotoUrl = "/imagenes/albums/" + nombreArchivo;
                }

                _context.Update(album);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlbumExists(album.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(album);
    }

    // GET: Albums/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var album = await _context.Albumes
            .Include(a => a.Cromos)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (album == null)
        {
            return NotFound();
        }

        return View(album);
    }

    // POST: Albums/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var album = await _context.Albumes.FindAsync(id);

        if (album != null)
        {
            if (!string.IsNullOrEmpty(album.FotoUrl))
            {
                string rutaImagen = Path.Combine(
                    _webHostEnvironment.WebRootPath,
                    album.FotoUrl.TrimStart('/'));

                if (System.IO.File.Exists(rutaImagen))
                {
                    System.IO.File.Delete(rutaImagen);
                }
            }

            _context.Albumes.Remove(album);
        }

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private bool AlbumExists(int id)
    {
        return _context.Albumes.Any(e => e.Id == id);
    }
}