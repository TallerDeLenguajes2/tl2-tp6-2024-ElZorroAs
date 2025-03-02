using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using repositoriosTP6;
using tl2_tp6_2024_ElZorroAs.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace tl2_tp6_2024_ElZorroAs.Controllers
{
    [AccessLevelAuthorize("Administrador", "Cliente")]
    public class PresupuestosController : Controller
    {
        private readonly IPresupuestoRepository _presupuestoRepository;
        private readonly IClientesRepository _clientesRepository;
        private readonly ILogger<PresupuestosController> _logger;

        public PresupuestosController(ILogger<PresupuestosController> logger, IPresupuestoRepository presupuestoRepository, IClientesRepository clientesRepository)
        {
            _logger = logger;
            _presupuestoRepository = presupuestoRepository;
            _clientesRepository = clientesRepository;
        }

        [HttpGet]
        public IActionResult ListarPresupuesto()
        {
            try
            {
                var presupuestos = _presupuestoRepository.ListarPresupuestos();

                if (User.IsInRole("Cliente"))
                {
                    var usuario = User.Identity.Name;
                    var cliente = _clientesRepository.ObtenerClientePorUsuario(usuario);

                    if (cliente != null)
                    {
                        presupuestos = presupuestos.Where(p => p.Cliente.ClienteId == cliente.ClienteId).ToList();
                    }
                    else
                    {
                        return Forbid(); // Si el cliente no se encuentra, prohibimos el acceso
                    }
                }

                return View(presupuestos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar presupuestos.");
                ViewBag.ErrorMessage = "Ocurrió un error al obtener la lista de presupuestos.";
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult VerPresupuesto(int id)
        {
            var presupuesto = _presupuestoRepository.ObtenerPresupuesto(id);

            if (presupuesto == null)
            {
                return NotFound();
            }

            return View(presupuesto);
        }

        [HttpGet]
        [AccessLevelAuthorize("Administrador")]
        public IActionResult CrearPresupuesto()
        {
            return View();
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessLevelAuthorize("Administrador")]
        public IActionResult CrearPresupuesto(Presupuestos model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                _presupuestoRepository.CrearPresupuesto(model);
                TempData["Mensaje"] = "Presupuesto creado con éxito.";
                return RedirectToAction("ListarPresupuesto");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear presupuesto.");
                ViewBag.ErrorMessage = "Ocurrió un error al crear el presupuesto.";
                return View("Error");
            }
        }

        [HttpGet]
        [AccessLevelAuthorize("Administrador")]
        public IActionResult ModificarPresupuesto(int id)
        {
            try
            {
                var presupuesto = _presupuestoRepository.ObtenerPresupuesto(id);
                if (presupuesto == null)
                {
                    return NotFound();
                }
                return View(presupuesto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el presupuesto para edición.");
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessLevelAuthorize("Administrador")]
        public IActionResult ModificarPresupuesto(Presupuestos model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                _presupuestoRepository.ModificarPresupuesto(model);
                TempData["Mensaje"] = "Presupuesto actualizado con éxito.";
                return RedirectToAction("ListarPresupuesto");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el presupuesto.");
                ViewBag.ErrorMessage = "Ocurrió un error al actualizar el presupuesto.";
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AccessLevelAuthorize("Administrador")]
        public IActionResult EliminarPresupuesto(int id)
        {
            try
            {
                _presupuestoRepository.EliminarPresupuesto(id);
                TempData["Mensaje"] = "Presupuesto eliminado con éxito.";
                return RedirectToAction("ListarPresupuesto");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar presupuesto.");
                ViewBag.ErrorMessage = "Ocurrió un error al eliminar el presupuesto.";
                return View("Error");
            }
        }
    }
}
