using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using repositoriosTP6;
using tl2_tp6_2024_ElZorroAs.Models;
using System;
using System.Collections.Generic;

namespace tl2_tp6_2024_ElZorroAs.Controllers
{
    public class PresupuestosController : Controller
    {
        private readonly IPresupuestoRepository _presupuestoRepository;
        private readonly IProductoRepository _productoRepository;
        private readonly ILogger<PresupuestosController> _logger;

        public PresupuestosController(
            ILogger<PresupuestosController> logger,
            IPresupuestoRepository presupuestoRepository,
            IProductoRepository productoRepository)
        {
            _logger = logger;
            _presupuestoRepository = presupuestoRepository;
            _productoRepository = productoRepository;
        }

        [HttpGet]
        public IActionResult ListarPresupuesto()
        {
            try
            {
                var presupuestos = _presupuestoRepository.ListarPresupuestos();
                _logger.LogInformation("Listado de presupuestos obtenido exitosamente.");
                return View(presupuestos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar presupuestos.");
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult CrearPresupuesto()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CrearPresupuesto(Clientes cliente)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("El modelo de cliente no es válido.");
                return RedirectToAction("ListarPresupuesto");
            }

            try
            {
                var presupuesto = new Presupuestos(cliente, DateTime.Now);
                _presupuestoRepository.CrearPresupuesto(presupuesto);
                _logger.LogInformation("Presupuesto creado exitosamente para el cliente {ClienteNombre}.", cliente.Nombre);
                return RedirectToAction("ListarPresupuesto");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear presupuesto.");
                return View("Error");
            }
        }

        public IActionResult VerPresupuesto(int id)
        {
            try
            {
                var presupuesto = _presupuestoRepository.ObtenerPresupuesto(id);
                if (presupuesto == null)
                {
                    _logger.LogWarning("Presupuesto con ID {IdPresupuesto} no encontrado.", id);
                    return NotFound();
                }
                _logger.LogInformation("Presupuesto con ID {IdPresupuesto} obtenido exitosamente.", id);
                return View(presupuesto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el presupuesto con ID {IdPresupuesto}.", id);
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult AgregarProducto(int idPresupuesto)
        {
            try
            {
                var presupuesto = _presupuestoRepository.ObtenerPresupuesto(idPresupuesto);
                if (presupuesto == null)
                {
                    _logger.LogWarning("Presupuesto con ID {IdPresupuesto} no encontrado.", idPresupuesto);
                    return NotFound();
                }

                ViewBag.IdPresupuesto = presupuesto.IdPresupuesto;
                ViewBag.ClienteNombre = presupuesto.Cliente.Nombre;
                ViewBag.Productos = _productoRepository.ListarProductos();

                _logger.LogInformation("Preparando para agregar producto al presupuesto con ID {IdPresupuesto}.", idPresupuesto);
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al preparar la adición de un producto al presupuesto con ID {IdPresupuesto}.", idPresupuesto);
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AgregarProductoPresupuesto(int idPresupuesto, int idProducto, int cantidad)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("El modelo de producto no es válido.");
                return RedirectToAction("ListarPresupuesto");
            }

            try
            {
                var producto = _productoRepository.ObtenerProducto(idProducto);
                if (producto == null)
                {
                    _logger.LogWarning("Producto con ID {IdProducto} no encontrado.", idProducto);
                    return NotFound("Producto no encontrado.");
                }

                _presupuestoRepository.AgregarProductoAPresupuesto(idPresupuesto, producto, cantidad);
                _logger.LogInformation("Producto con ID {IdProducto} agregado al presupuesto con ID {IdPresupuesto} exitosamente.", idProducto, idPresupuesto);

                return RedirectToAction("ListarPresupuesto");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar producto al presupuesto con ID {IdPresupuesto}.", idPresupuesto);
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult EliminarPresupuesto(int id)
        {
            try
            {
                _presupuestoRepository.EliminarPresupuesto(id);
                _logger.LogInformation("Presupuesto con ID {IdPresupuesto} eliminado exitosamente.", id);
                return RedirectToAction("ListarPresupuesto");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el presupuesto con ID {IdPresupuesto}.", id);
                return View("Error");
            }
        }
    }
}