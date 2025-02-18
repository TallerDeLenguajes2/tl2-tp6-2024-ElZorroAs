using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_ElZorroAs.Models;
using repositoriosTP6;

namespace tl2_tp6_2024_ElZorroAs.Controllers
{
    public class ProductosController : Controller
    {
        private readonly IProductoRepository _productoRepository;

        public ProductosController(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }

        // Acción para listar productos
        public IActionResult ListarProductos()
        {
            var productos = _productoRepository.ListarProductos();
            return View(productos);
        }

        // Acción para obtener un producto
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

        // Acción para recibir datos POST de un producto
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
            var producto = new Productos(descripcion, precio);
            _productoRepository.CrearProducto(producto);
            return RedirectToAction("ListarProductos");
        }



        // Acción para mostrar la vista de modificar producto
        [HttpGet]
        // Acción para mostrar la vista de modificar producto
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

        // Acción para modificar un producto
        [HttpPost]
        public IActionResult ModificarProducto(int id, string descripcion, int precio)
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

        // Acción para mostrar la vista de eliminar producto
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

        // Acción para confirmar la eliminación de un producto
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
