using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // Asegúrate de importar el espacio de nombres para el logger
using tl2_tp6_2024_ElZorroAs.Models;
using repositoriosTP6;

namespace tl2_tp6_2024_ElZorroAs.Controllers
{
    public class ProductosController : Controller
    {
        private readonly IProductoRepository _productoRepository;
        private readonly ILogger<ProductosController> _logger; // Logger

        public ProductosController(IProductoRepository productoRepository, ILogger<ProductosController> logger)
        {
            _productoRepository = productoRepository;
            _logger = logger; // Inicializamos el logger
        }

        // Acción para listar productos
        public IActionResult ListarProductos()
        {
            try
            {
                var productos = _productoRepository.ListarProductos();
                return View(productos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar productos.");
                return View("Error"); // Retorna una vista de error
            }
        }

        // Acción para obtener un producto
        [HttpGet]
        public IActionResult ObtenerProducto(int id)
        {
            try
            {
                var producto = _productoRepository.ObtenerProducto(id);
                if (producto == null)
                {
                    return NotFound();
                }
                return View(producto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el producto con ID {Id}.", id);
                return View("Error"); // Retorna una vista de error
            }
        }

        // Acción para recibir datos POST de un producto
        [HttpPost]
        public IActionResult ObtenerProductoPost(int id)
        {
            try
            {
                var producto = _productoRepository.ObtenerProducto(id);
                if (producto == null)
                {
                    return NotFound();
                }
                return View("ObtenerProducto", producto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el producto con ID {Id} en la acción POST.", id);
                return View("Error"); // Retorna una vista de error
            }
        }

        // Acción para mostrar la vista de crear producto
        [HttpGet]
        public IActionResult CrearProducto()
        {
            return View();
        }

        // Acción para procesar la creación de un producto
        [HttpPost]
        public IActionResult CrearProducto(string descripcion, int precio)
        {
            try
            {
                var producto = new Productos(descripcion, precio);
                _productoRepository.CrearProducto(producto);
                return RedirectToAction("ListarProductos");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear el producto.");
                return View("Error"); // Retorna una vista de error
            }
        }

        // Acción para mostrar la vista de modificar producto
        [HttpGet]
        public IActionResult ModificarProducto(int id)
        {
            try
            {
                var producto = _productoRepository.ObtenerProducto(id);
                if (producto == null)
                {
                    return NotFound();
                }
                return View(producto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar la vista de modificación del producto con ID {Id}.", id);
                return View("Error"); // Retorna una vista de error
            }
        }

        // Acción para modificar un producto
        [HttpPost]
        public IActionResult ModificarProducto(int id, string descripcion, int precio)
        {
            try
            {
                var productoExistente = _productoRepository.ObtenerProducto(id);
                if (productoExistente == null)
                {
                    return NotFound();
                }

                // Crear una nueva instancia del modelo con los valores recibidos
                var productoModificado = new Productos(id, descripcion, precio);

                // Llamar al repositorio para modificar el producto
                _productoRepository.ModificarProducto(id, productoModificado);

                return RedirectToAction("ListarProductos");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al modificar el producto con ID {Id}.", id);
                return View("Error"); // Retorna una vista de error
            }
        }

        // Acción para mostrar la vista de eliminar producto
        [HttpGet]
        public IActionResult EliminarProducto(int id)
        {
            try
            {
                var producto = _productoRepository.ObtenerProducto(id);
                if (producto == null)
                {
                    return NotFound();
                }
                return View(producto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar la vista de eliminación del producto con ID {Id}.", id);
                return View("Error"); // Retorna una vista de error
            }
        }

        // Acción para confirmar la eliminación de un producto
        [HttpPost]
        public IActionResult ConfirmarEliminarProducto(int id)
        {
            try
            {
                var producto = _productoRepository.ObtenerProducto(id);
                if (producto == null)
                {
                    return NotFound();
                }
                _productoRepository.EliminarProducto(id);
                return RedirectToAction("ListarProductos");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el producto con ID {Id}.", id);
                return View("Error"); // Retorna una vista de error
            }
        }
    }
}