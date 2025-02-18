using tl2_tp6_2024_ElZorroAs.Models;
using System.Collections.Generic;

namespace repositoriosTP6
{
    public interface IProductoRepository
    {
        public void CrearProducto(Productos producto); 
        public void ModificarProducto(int id, Productos producto);
        public List<Productos> ListarProductos();
        public Productos ObtenerProducto(int id);
        public void EliminarProducto(int id);
    }
}