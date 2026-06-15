
using albumMundial.Data;
using albumMundial.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class CromosController : Controller
{
    private readonly ApplicationDbContext _context;

    public CromosController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: CROMOS
    public async Task<IActionResult> Index(string? buscar)    
    {
        /*
        var cromos = await _context.Cromos
        .Include(c => c.Jugador)
        .Include(c => c.Album)    
        .ToListAsync();

        return View(cromos);


          var equipos = _context.Equipos
            .Include(e => e.Pais)
            .AsQueryable();

        if (!string.IsNullOrEmpty(buscar))
            equipos = equipos.Where(e => e.Nombre.Contains(buscar)
                                     || e.Pais.Nombre.Contains(buscar));

        ViewBag.Buscar = buscar;
        return View(await equipos.ToListAsync());

        */

        var query = _context.Cromos
         .Include(c => c.Jugador)
         .Include(c => c.Album)
         .AsQueryable();

        if (!string.IsNullOrEmpty(buscar))
        {
            query = query.Where(e => e.Jugador.Nombre.Contains(buscar));
        }

        ViewBag.Buscar = buscar;

        var listaCromos = await query.ToListAsync();

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return PartialView(listaCromos);
        }

        return View(listaCromos);

    }

    // GET: CROMOS/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cromo = await _context.Cromos
            .Include(c => c.Jugador)
            .Include(c => c.Album)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (cromo == null)
        {
            return NotFound();
        }

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return PartialView(cromo);
        }


        return View(cromo);
    }

    // GET: CROMOS/Create
    public IActionResult Create()
    {
        ViewData["AlbumId"] = new SelectList(_context.Albumes, "Id", "Nombre");
        ViewData["JugadorId"] = new SelectList(_context.Jugadores, "Id", "Nombre");

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return PartialView();
        }

        return View();
    }

    // POST: CROMOS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,NumeroCromo,Edicion,ValorMercado,JugadorId,AlbumId")] Cromo cromo, IFormFile imagenCromo)
    {
        if (ModelState.IsValid)
        {
            // Verificar si el usuario subió una imagen
            if (imagenCromo != null && imagenCromo.Length > 0)
            {
                // Definir la ruta de la carpeta donde se guardará (wwwroot/images/cromos)
                string carpetaDestino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "cromos");

                // Crear la carpeta si no existe
                if (!Directory.Exists(carpetaDestino))
                {
                    Directory.CreateDirectory(carpetaDestino);
                }

                // Generar un nombre único para evitar que se sobrescriban imágenes con el mismo nombre
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imagenCromo.FileName);
                var filePath = Path.Combine(carpetaDestino, fileName);

                // Guardar físicamente el archivo en el servidor
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imagenCromo.CopyToAsync(stream);
                }

                // Guardar la ruta web relativa en la base de datos
                cromo.FotoUrl = "/images/cromos/" + fileName;
            }

            _context.Add(cromo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["AlbumId"] = new SelectList(_context.Albumes, "Id", "Nombre", cromo.AlbumId);
        ViewData["JugadorId"] = new SelectList(_context.Jugadores, "Id", "Nombre", cromo.JugadorId);
        return View(cromo);
    }

    // GET: CROMOS/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cromo = await _context.Cromos.FindAsync(id);
        if (cromo == null)
        {
            return NotFound();
        }

        // MANDAMOS LOS SELECTS CON EL VALOR SELECCIONADO ACTUALMENTE
        ViewData["AlbumId"] = new SelectList(_context.Albumes, "Id", "Nombre", cromo.AlbumId);
        ViewData["JugadorId"] = new SelectList(_context.Jugadores, "Id", "Nombre", cromo.JugadorId);


        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return PartialView(cromo);
        }

        return View(cromo);
    }

    // POST: CROMOS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,NumeroCromo,Edicion,ValorMercado,FotoUrl,JugadorId,AlbumId")] Cromo cromo, IFormFile? nuevaImagenCromo) // 👈 Agregar el '?' aquí
    {
        if (id != cromo.Id)
        {
            return NotFound();
        }

        // 🟢 Limpieza preventiva de validaciones de formato decimal
        if (cromo.ValorMercado > 0)
        {
            ModelState.Remove("ValorMercado");
        }

        // 🟢 SOLUCIÓN AL BLOQUEO: Forzamos a remover el archivo de las reglas obligatorias
        ModelState.Remove("nuevaImagenCromo");

        if (ModelState.IsValid)
        {
            try
            {
                // Si el usuario seleccionó una NUEVA foto, la procesamos
                if (nuevaImagenCromo != null && nuevaImagenCromo.Length > 0)
                {
                    string carpetaDestino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "cromos");

                    if (!Directory.Exists(carpetaDestino))
                    {
                        Directory.CreateDirectory(carpetaDestino);
                    }

                    string nombreArchivoUnico = Guid.NewGuid().ToString() + Path.GetExtension(nuevaImagenCromo.FileName);
                    string rutaCompleta = Path.Combine(carpetaDestino, nombreArchivoUnico);

                    using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                    {
                        await nuevaImagenCromo.CopyToAsync(stream);
                    }

                    // Asignamos la nueva ruta web
                    cromo.FotoUrl = "/images/cromos/" + nombreArchivoUnico;
                }
                // Si viene nulo, conservará el valor del input hidden de 'FotoUrl' intacto

                _context.Update(cromo);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Cromos.Any(e => e.Id == cromo.Id))
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

        // Si algo falla, recargamos los selects para no romper la vista
        ViewData["AlbumId"] = new SelectList(_context.Albumes, "Id", "Nombre", cromo.AlbumId);
        ViewData["JugadorId"] = new SelectList(_context.Jugadores, "Id", "Nombre", cromo.JugadorId);
        return View(cromo);
    }

    // GET: CROMOS/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cromo = await _context.Cromos
        .Include(c => c.Jugador) 
        .Include(c => c.Album) 
        .FirstOrDefaultAsync(m => m.Id == id);
        if (cromo == null)
        {
            return NotFound();
        }

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return PartialView(cromo);
        }

        return View(cromo);
    }

    // POST: CROMOS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var cromo = await _context.Cromos.FindAsync(id);
        if (cromo != null)
        {
            _context.Cromos.Remove(cromo);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CromoExists(int? id)
    {
        return _context.Cromos.Any(e => e.Id == id);
    }
}
