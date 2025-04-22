using FreshBites.Data;
using FreshBites.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace FreshBites.Controllers
{
    public class ProductosController : Controller
    {
        private readonly FreshBitesDbContext _context;

        public ProductosController(FreshBitesDbContext context)
        {
            _context = context;
        }

        // GET: Productos
        public async Task<IActionResult> Index(int page = 1, int pageSize = 6)
        {
            var totalProductos = await _context.Productos.CountAsync();
            var productos = await _context.Productos
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalProductos / (double)pageSize);

            return View(productos);
        }


        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Gestion()
        {
            var productos = await _context.Productos.ToListAsync();
            return View(productos);
        }


        [Authorize(Roles = "Administrador")]
        public IActionResult Crear() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Producto producto)
        {
            if (producto.ImagenArchivo != null && producto.ImagenArchivo.Length > 0)
            {
                string rutaCarpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                Directory.CreateDirectory(rutaCarpeta);

                string nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(producto.ImagenArchivo.FileName);
                string rutaCompleta = Path.Combine(rutaCarpeta, nombreArchivo);

                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    await producto.ImagenArchivo.CopyToAsync(stream);
                }

                producto.ImagenUrl = "/images/" + nombreArchivo;
            }

            
            ModelState.Remove(nameof(producto.ImagenUrl));

            if (ModelState.IsValid)
            {
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(producto);
        }



        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null) return NotFound();

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound();

            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Producto producto)
        {
            if (id != producto.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null) return NotFound();

            var producto = await _context.Productos.FirstOrDefaultAsync(p => p.Id == id);
            if (producto == null) return NotFound();

            return View(producto);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }

}
