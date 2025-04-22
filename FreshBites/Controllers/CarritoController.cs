using FreshBites.Data;
using FreshBites.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FreshBites.Controllers
{
    public class CarritoController : Controller
    {
        private readonly FreshBitesDbContext _context;
        private static List<CarritoItem> _carrito = new List<CarritoItem>();

        public CarritoController(FreshBitesDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_carrito);
        }

        [Authorize]
        public async Task<IActionResult> Agregar(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound();

            var item = _carrito.FirstOrDefault(c => c.Producto.Id == id);
            if (item != null)
            {
                item.Cantidad++;
            }
            else
            {
                _carrito.Add(new CarritoItem { Producto = producto, Cantidad = 1 });
            }

            return RedirectToAction("Index", "Productos");
        }

        public IActionResult Eliminar(int id)
        {
            var item = _carrito.FirstOrDefault(c => c.Producto.Id == id);
            if (item != null)
            {
                _carrito.Remove(item);
            }

            return RedirectToAction("Index");
        }

        public IActionResult ConfirmarCompra()
        {
            _carrito.Clear();
            TempData["CompraExitosa"] = "¡Gracias por tu compra!";
            return RedirectToAction("Index", "Productos");
        }
    }

}
