using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using tl2_tp6_2024_ElZorroAs.Models;
using rapositoriosTP5;

namespace tl2_tp6_2024_ElZorroAs.Controllers
{
    public class ProductosController : Controller
    {
        private readonly IProductoRepository _productoRepository;

        public ProductosController(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }

        public IActionResult ListarProductos()
        {
            var productos = _productoRepository.ListarProductos();
            return View(productos);
        }

        [HttpGet]
        public IActionResult ObtenerProducto(int id)
        {
            var producto = _productoRepository.ObtenerProducto(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        [HttpPost]
        public IActionResult ObtenerProductoPost(int id)
        {
            var producto = _productoRepository.ObtenerProducto(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View("ObtenerProducto", producto);
        }

        [HttpGet]
        public IActionResult CrearProducto()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CrearProducto(Productos producto)
        {
            _productoRepository.CrearProducto(producto);
            return RedirectToAction("ListarProductos");
        }

        [HttpGet]
        public IActionResult ModificarProducto(int id)
        {
            var producto = _productoRepository.ObtenerProducto(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        [HttpPost]
        public IActionResult ModificarProducto(int id, Productos producto)
        {
            var existeProducto = _productoRepository.ObtenerProducto(id);
            if (existeProducto == null)
            {
                return NotFound();
            }
            _productoRepository.ModificarProducto(id, producto);
            return RedirectToAction("ListarProductos");
        }

        [HttpGet]
        public IActionResult EliminarProducto(int id)
        {
            var producto = _productoRepository.ObtenerProducto(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }

        [HttpPost]
        public IActionResult ConfirmarEliminarProducto(int id)
        {
            var producto = _productoRepository.ObtenerProducto(id);
            if (producto == null)
            {
                return NotFound();
            }
            _productoRepository.EliminarProducto(id);
            return RedirectToAction("ListarProductos");
        }
    }
}