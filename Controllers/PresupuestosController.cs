using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // Importamos el espacio de nombres para el logger
using repositoriosTP6;
using tl2_tp6_2024_ElZorroAs.Models;
using System;
using System.Collections.Generic;


namespace tl2_tp6_2024_ElZorroAs.Controllers
{
    public class PresupuestosController : Controller
    {
        private readonly IPresupuestoRepository _presupuestoRepository;//cambie aqui
        private readonly IProductoRepository _productoRepository;//cambie aqui
        private readonly ILogger<PresupuestosController> _logger; // Definimos el logger
        // Constructor con inyección de dependencias
        public PresupuestosController(//cambie aqui
    ILogger<PresupuestosController> logger,
    IPresupuestoRepository presupuestoRepository,
    IProductoRepository productoRepository) // Asegurar que recibe la interfaz
        {
            _logger = logger;
            _presupuestoRepository = presupuestoRepository;//cambie aqui
            _productoRepository = productoRepository;//cambie aqui
        }


        public IActionResult ListarPresupuesto()
        {
            var presupuestos = _presupuestoRepository.ListarPresupuestos();
            return View(presupuestos);
        }

        [HttpGet]
        public IActionResult CrearPresupuesto()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CrearPresupuesto(string nombreDestinatario)
        {
            var presupuesto = new Presupuestos(nombreDestinatario, DateTime.Now);
            _presupuestoRepository.CrearPresupuesto(presupuesto);
            return RedirectToAction("ListarPresupuesto");
        }

        public IActionResult VerPresupuesto(int id)
        {
            var presupuesto = _presupuestoRepository.ObtenerPresupuesto(id);
            if (presupuesto == null)
                return NotFound();
            return View(presupuesto);
        }
        //cambie este
        [HttpGet]
        public IActionResult AgregarProductoPresupuesto(int id)
        {
            try
            {
                var presupuesto = _presupuestoRepository.ObtenerPresupuesto(id);
                if (presupuesto == null)
                {
                    _logger.LogWarning("Presupuesto con ID {Id} no encontrado", id);
                    return NotFound("Presupuesto no encontrado.");
                }

                ViewBag.Productos = _productoRepository.ListarProductos(); // Lista de productos disponibles
                ViewBag.IdPresupuesto = id; // Pasar el ID para la vista
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar productos para el presupuesto con ID {Id}", id);
                return View("Error");
            }
        }
        // y este
        // POST: /Presupuestos/AgregarProductoPresupuesto
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AgregarProductoPresupuesto(int idPresupuesto, int idProducto, int cantidad)

        {
            try
            {
                var producto = _productoRepository.ObtenerProducto(idProducto);
                if (producto == null)
                {
                    _logger.LogWarning("Producto con ID {IdProducto} no encontrado", idProducto);
                    return NotFound("Producto no encontrado.");
                }

                _presupuestoRepository.AgregarProductoAPresupuesto(idPresupuesto, producto, cantidad);
                _logger.LogInformation("Producto con ID {IdProducto} agregado al presupuesto con ID {IdPresupuesto}", idProducto, idPresupuesto);

                return RedirectToAction("ListarPresupuesto");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar producto al presupuesto con ID {IdPresupuesto}", idPresupuesto);
                return View("Error");
            }
        }
        public IActionResult EliminarPresupuesto(int id)
        {
            _presupuestoRepository.EliminarPresupuesto(id);
            return RedirectToAction("ListarPresupuesto");
        }
    }
}

