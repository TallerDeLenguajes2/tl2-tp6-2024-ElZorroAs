using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using repositoriosTP6;
using tl2_tp6_2024_ElZorroAs.Models;
using System;
using System.Collections.Generic;

namespace tl2_tp6_2024_ElZorroAs.Controllers
{
    public class ClientesController : Controller
    {
        private readonly IClientesRepository _clientesRepository;
        private readonly ILogger<ClientesController> _logger;

        public ClientesController(
            ILogger<ClientesController> logger,
            IClientesRepository clientesRepository)
        {
            _logger = logger;
            _clientesRepository = clientesRepository;
        }

        [HttpGet]
        public IActionResult ListarClientes()
        {
            try
            {
                var clientes = _clientesRepository.ListarClientes();
                _logger.LogInformation("Listado de clientes obtenido exitosamente.");
                return View(clientes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar clientes.");
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult CrearCliente()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CrearCliente(string nombre, string email, string telefono)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("El modelo de cliente no es válido.");
                return View(); // Regresa a la vista con el estado actual
            }

            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("", "El nombre y el email son obligatorios.");
                return View(); // Retorna la vista con el error
            }

            var cliente = new Clientes(nombre, email, telefono);

            try
            {
                _clientesRepository.CrearCliente(cliente);
                _logger.LogInformation("Cliente creado exitosamente: {ClienteNombre}.", cliente.Nombre);
                return RedirectToAction("ListarClientes");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear cliente.");
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult ModificarCliente(int id)
        {
            try
            {
                var cliente = _clientesRepository.ObtenerCliente(id);
                if (cliente == null)
                {
                    _logger.LogWarning("Cliente con ID {ClienteId} no encontrado.", id);
                    return NotFound();
                }
                return View(cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el cliente con ID {ClienteId} para modificación.", id);
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ModificarCliente(int idCliente, string nombre, string email, string telefono)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("El modelo de cliente no es válido.");
                return View();
            }

            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("", "El nombre y el email son obligatorios.");
                return View();
            }

            var clienteActualizar = new Clientes(idCliente, nombre, email, telefono);

            try
            {
                _clientesRepository.ModificarCliente(idCliente, clienteActualizar);
                _logger.LogInformation("Cliente con ID {ClienteId} modificado exitosamente.", idCliente);
                return RedirectToAction("ListarClientes");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al modificar el cliente con ID {ClienteId}.", idCliente);
                return View("Error");
            }
        }
        [HttpPost]
        public IActionResult EliminarCliente(int id)
        {
            try
            {
                _clientesRepository.EliminarCliente(id);
                _logger.LogInformation("Cliente con ID {ClienteId} eliminado exitosamente.", id);
                return RedirectToAction("ListarClientes");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el cliente con ID {ClienteId}.", id);
                return View("Error");
            }
        }
    }
}