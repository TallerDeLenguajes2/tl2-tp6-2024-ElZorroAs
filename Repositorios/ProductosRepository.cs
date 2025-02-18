using tl2_tp6_2024_ElZorroAs.Models;
using Microsoft.Data.Sqlite;
namespace repositoriosTP6
{
    public class ProductoRepository : IProductoRepository
    {
        private string cadenaConexion = "Data Source=DB/tienda.db;Cache=Shared";

        public void CrearProducto(Productos producto)
        {
            var query = "INSERT INTO productos (descripcion, precio) VALUES (@descripcion, @precio)";
            using (var connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@descripcion", producto.Descripcion);
                    command.Parameters.AddWithValue("@precio", producto.Precio);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public void ModificarProducto(int id, Productos producto)
        {
            var query = "UPDATE productos SET descripcion = @descripcion, precio = @precio WHERE idProducto = @idProducto";
            using (var connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idProducto", id);
                    command.Parameters.AddWithValue("@descripcion", producto.Descripcion);
                    command.Parameters.AddWithValue("@precio", producto.Precio);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public List<Productos> ListarProductos()
        {
            var productos = new List<Productos>();
            var query = "SELECT * FROM productos";
            using (var connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();
                using (var command = new SqliteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var producto = new Productos(
                                Convert.ToInt32(reader["idProducto"]),
                                reader["descripcion"].ToString(),
                                Convert.ToInt32(reader["precio"])
                            );
                            productos.Add(producto);
                        }
                    }
                }
                connection.Close();
            }
            return productos;
        }

        public Productos ObtenerProducto(int id)
        {
            Productos producto = null;
            var query = "SELECT * FROM productos WHERE idProducto = @idProducto";
            using (var connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idProducto", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            producto = new Productos(
                                Convert.ToInt32(reader["idProducto"]),
                                reader["descripcion"].ToString(),
                                Convert.ToInt32(reader["precio"])
                            );
                        }
                    }
                }
                connection.Close();
            }
            return producto;
        }

        public void EliminarProducto(int id)
        {
            var query = "DELETE FROM productos WHERE idProducto = @idProducto";
            using (var connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idProducto", id);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
    }
}
