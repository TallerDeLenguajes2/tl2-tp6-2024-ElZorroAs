using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging; // Importamos el espacio de nombres para el logger
using tl2_tp6_2024_ElZorroAs.Models;

namespace repositoriosTP6
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly string cadenaConexion;
        private readonly ILogger<ProductoRepository> _logger; // Logger

        public ProductoRepository(string cadenaDeConexion, ILogger<ProductoRepository> logger)
        {
            cadenaConexion = cadenaDeConexion;
            _logger = logger; // Inicializamos el logger
        }

        public void CrearProducto(Productos producto)
        {
            try
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
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString()); // Logueo del error
                throw new Exception("Error al crear el producto", ex);
            }
        }

        public void ModificarProducto(int id, Productos producto)
        {
            try
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
                        int filasAfectadas = command.ExecuteNonQuery();

                        if (filasAfectadas == 0)
                        {
                            throw new Exception($"No se encontró el producto con ID: {id} para modificar.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString()); // Logueo del error
                throw new Exception("Error al modificar el producto", ex);
            }
        }

        public List<Productos> ListarProductos()
        {
            var productos = new List<Productos>();
            var query = "SELECT * FROM productos";
            try
            {
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
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString()); // Logueo del error
                throw new Exception("Error al listar los productos", ex);
            }
            return productos;
        }

        public Productos ObtenerProducto(int id)
        {
            Productos producto = null;
            var query = "SELECT * FROM productos WHERE idProducto = @idProducto";
            try
            {
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
                            else
                            {
                                throw new Exception($"No se encontró el producto con ID: {id}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString()); // Logueo del error
                throw new Exception("Error al obtener el producto", ex);
            }
            return producto;
        }

        public void EliminarProducto(int id)
        {
            try
            {
                using (var connection = new SqliteConnection(cadenaConexion))
                {
                    connection.Open();

                    // Eliminar primero las referencias en PresupuestosDetalle
                    var queryEliminarReferencias = "DELETE FROM PresupuestosDetalle WHERE idProducto = @idProducto";
                    using (var command = new SqliteCommand(queryEliminarReferencias, connection))
                    {
                        command.Parameters.AddWithValue("@idProducto", id);
                        int filasAfectadas = command.ExecuteNonQuery();
                        if (filasAfectadas == 0)
                        {
                            throw new Exception($"No se encontraron referencias para el producto con ID: {id}");
                        }
                    }

                    // Luego, eliminar el producto
                    var queryEliminarProducto = "DELETE FROM productos WHERE idProducto = @idProducto";
                    using (var command = new SqliteCommand(queryEliminarProducto, connection))
                    {
                        command.Parameters.AddWithValue("@idProducto", id);
                        int filasAfectadas = command.ExecuteNonQuery();
                        if (filasAfectadas == 0)
                        {
                            throw new Exception($"No se encontró el producto con ID: {id} para eliminar.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString()); // Logueo del error
                throw new Exception("Error al eliminar el producto", ex);
            }
        }

        public void Acceso(string usuarioLogueado, string clave)
        {
            // Simulación de un método de acceso
            try
            {
                // Aquí iría la lógica para verificar el acceso
                bool accesoExitoso = true; // Cambia esto según tu lógica

                if (accesoExitoso)
                {
                    _logger.LogInformation($"El usuario {usuarioLogueado} ingresó correctamente."); // Logueo de acceso exitoso
                }
                else
                {
                    _logger.LogWarning($"Intento de acceso inválido - Usuario: {usuarioLogueado} Clave ingresada: {clave}"); // Logueo de acceso fallido
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString()); // Logueo del error
                throw;
            }
        }
    }
}