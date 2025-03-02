using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using repositoriosTP6;
using tl2_tp6_2024_ElZorroAs.Models;
using tl2_tp6_2024_ElZorroAs.ViewModels;
using System;
using System.Collections.Generic;

namespace tl2_tp6_2024_ElZorroAs.Controllers
{
    public class PresupuestosController : Controller
    {
        private readonly IPresupuestoRepository _presupuestoRepository;
        private readonly IProductoRepository _productoRepository;
        private readonly IClientesRepository _clienteRepository;
        private readonly ILogger<PresupuestosController> _logger;

        public PresupuestosController(
            ILogger<PresupuestosController> logger,
            IPresupuestoRepository presupuestoRepository,
            IProductoRepository productoRepository, IClientesRepository clienteRepository)
        {
            _logger = logger;
            _presupuestoRepository = presupuestoRepository;
            _productoRepository = productoRepository;
            _clienteRepository = clienteRepository;
        }

        [AccessLevelAuthorize("Administrador", "Cliente")]
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
        [AccessLevelAuthorize("Administrador")]
        [HttpGet]
        public IActionResult CrearPresupuesto()
        {
            try
            {
                var viewModel = new PresupuestoViewModel
                {
                    ClientesDisponibles = _clienteRepository.ListarClientes()
                };
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al preparar la creación del presupuesto.");
                return View("Error");
            }
        }
        [AccessLevelAuthorize("Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CrearPresupuesto(PresupuestoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("El modelo de presupuesto no es válido.");
                model.ClientesDisponibles = _clienteRepository.ListarClientes() ?? new List<Clientes>(); // 🔹 Asegura que no sea null
                return View(model);
            }

            try
            {
                var cliente = _clienteRepository.ObtenerCliente(model.ClienteId);
                if (cliente == null)
                {
                    _logger.LogWarning("Cliente con ID {ClienteId} no encontrado.", model.ClienteId);
                    return NotFound("Cliente no encontrado.");
                }

                var presupuesto = new Presupuestos(cliente, model.FechaCreacion);
                _presupuestoRepository.CrearPresupuesto(presupuesto);
                _logger.LogInformation("Presupuesto creado exitosamente para el cliente {ClienteNombre}.", cliente.Nombre);
                return RedirectToAction("ListarPresupuesto");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear presupuesto.");
                model.ClientesDisponibles = _clienteRepository.ListarClientes() ?? new List<Clientes>(); // 🔹 Evita el null en caso de error
                return View(model);
            }
        }
        [AccessLevelAuthorize("Administrador", "Cliente")]
        [HttpGet]
        public IActionResult VerPresupuesto(int id)
        {
            try
            {
                var presupuesto = _presupuestoRepository.ObtenerPresupuesto(id);
                if (presupuesto == null)
                {
                    _logger.LogWarning("Presupuesto con ID {IdPresupuesto} no encontrado.", id);
                    return NotFound("Presupuesto no encontrado.");
                }

                return View(presupuesto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el presupuesto con ID {IdPresupuesto}.", id);
                return View("Error");
            }
        }
        [AccessLevelAuthorize("Administrador")]
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

                var viewModel = new AgregarProductoPresupuestoViewModel
                {
                    IdPresupuesto = idPresupuesto,
                    ProductosDisponibles = _productoRepository.ListarProductos()
                };

                return View("AgregarProductoPresupuesto", viewModel); // Aseguramos que llama a la vista correcta
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al preparar la adición de un producto al presupuesto con ID {IdPresupuesto}.", idPresupuesto);
                return View("Error");
            }
        }

        [AccessLevelAuthorize("Administrador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AgregarProductoPresupuesto(AgregarProductoPresupuestoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("El modelo de producto no es válido.");
                model.ProductosDisponibles = _productoRepository.ListarProductos();
                return View("AgregarProductoPresupuesto", model); // Aseguramos que llama a la vista correcta
            }

            try
            {
                var producto = _productoRepository.ObtenerProducto(model.ProductoId);
                if (producto == null)
                {
                    _logger.LogWarning("Producto con ID {ProductoId} no encontrado.", model.ProductoId);
                    return NotFound("Producto no encontrado.");
                }

                _presupuestoRepository.AgregarProductoAPresupuesto(model.IdPresupuesto, producto, model.Cantidad);
                _logger.LogInformation("Producto con ID {ProductoId} agregado al presupuesto con ID {IdPresupuesto} exitosamente.", model.ProductoId, model.IdPresupuesto);

                return RedirectToAction("ListarPresupuesto");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar producto al presupuesto con ID {IdPresupuesto}.", model.IdPresupuesto);
                return View("Error");
            }
        }
        [AccessLevelAuthorize("Administrador")]
        [HttpGet]
        public IActionResult ModificarPresupuesto(int id)
        {
            try
            {
                var presupuesto = _presupuestoRepository.ObtenerPresupuesto(id);
                if (presupuesto == null)
                {
                    _logger.LogWarning($"Presupuesto con ID {id} no encontrado.");
                    return NotFound();
                }

                var viewModel = new PresupuestoViewModel
                {
                    IdPresupuesto = presupuesto.IdPresupuesto, // 🔹 Ahora el ViewModel tiene el ID del presupuesto
                    ClienteId = presupuesto.Cliente.ClienteId,
                    FechaCreacion = presupuesto.FechaCreacion,
                    ClientesDisponibles = _clienteRepository.ListarClientes(),
                    Detalle = presupuesto.Detalle
                };

                return View("ModificarPresupuesto", viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar la vista de modificación.");
                return StatusCode(500);
            }
        }

        [AccessLevelAuthorize("Administrador")]
        [HttpPost]
        [Route("Presupuestos/ModificarPresupuesto")]
        public IActionResult ModificarPresupuesto(int IdPresupuesto, DateTime FechaCreacion, int[] IdProductos, int[] Cantidades)
        {
            if (IdProductos == null || Cantidades == null || IdProductos.Length != Cantidades.Length)
            {
                _logger.LogWarning("Error en la estructura de los datos enviados.");
                return RedirectToAction("ListarPresupuesto");
            }

            try
            {
                var presupuestoExistente = _presupuestoRepository.ObtenerPresupuesto(IdPresupuesto); // ✅ CORREGIDO: Se usa IdPresupuesto en vez de ClienteId
                if (presupuestoExistente == null)
                {
                    _logger.LogWarning($"Presupuesto con ID {IdPresupuesto} no encontrado.");
                    return NotFound();
                }

                var nuevoDetalle = new List<PresupuestosDetalle>();
                for (int i = 0; i < IdProductos.Length; i++)
                {
                    var producto = _productoRepository.ObtenerProducto(IdProductos[i]);
                    if (producto != null && Cantidades[i] > 0)
                    {
                        var detallePresupuesto = new PresupuestosDetalle(producto, Cantidades[i]);
                        nuevoDetalle.Add(detallePresupuesto);
                    }
                }

                var presupuestoModificado = new Presupuestos(
                    presupuestoExistente.IdPresupuesto,
                    presupuestoExistente.Cliente,
                    FechaCreacion,
                    nuevoDetalle
                );

                _presupuestoRepository.ModificarPresupuesto(presupuestoModificado);
                return RedirectToAction("ListarPresupuesto");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al modificar el presupuesto.");
                return StatusCode(500);
            }
        }


        [AccessLevelAuthorize("Administrador")]
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