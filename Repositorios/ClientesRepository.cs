using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging; // Importamos el espacio de nombres para el logger
using tl2_tp6_2024_ElZorroAs.Models;

namespace repositoriosTP6
{
    public class ClientesRepository : IClientesRepository
    {
        private readonly string cadenaConexion;
        private readonly ILogger<ClientesRepository> _logger; // Logger

        public ClientesRepository(string cadenaDeConexion,ILogger<ClientesRepository> logger)
        {
            cadenaConexion = cadenaDeConexion;
            _logger = logger; // Inicializamos el logger
        }

        public void CrearCliente(Clientes cliente)
        {
            try
            {
                var query = "INSERT INTO clientes (nombre, email, telefono) VALUES (@nombre, @email, @telefono)";
                using (var connection = new SqliteConnection(cadenaConexion))
                {
                    connection.Open();
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nombre", cliente.Nombre);
                        command.Parameters.AddWithValue("@email", cliente.Email);
                        command.Parameters.AddWithValue("@telefono", cliente.Telefono);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString()); // Logueo del error
                throw new Exception("Error al crear el cliente", ex);
            }
        }

        public Clientes ObtenerClientePorUsuario(string usuarioNombre)
        {
            try
            {
                Clientes cliente = null;
                var query = @"SELECT c.ClienteId, c.nombre, c.email, c.telefono 
                              FROM clientes c
                              INNER JOIN usuarios u ON c.ClienteId = u.ClienteId
                              WHERE u.Usuario = @usuarioNombre";

                using (var connection = new SqliteConnection(cadenaConexion))
                {
                    connection.Open();
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@usuarioNombre", usuarioNombre);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                cliente = new Clientes(
                                    Convert.ToInt32(reader["ClienteId"]),
                                    reader["nombre"].ToString(),
                                    reader["email"].ToString(),
                                    reader["telefono"].ToString()
                                );
                                _logger.LogInformation($"El usuario {usuarioNombre} ingresó correctamente."); // Logueo de acceso exitoso
                            }
                            else
                            {
                                throw new Exception($"No se encontró un cliente para el usuario: {usuarioNombre}");
                            }
                        }
                    }
                }
                return cliente;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString()); // Logueo del error
                throw new Exception("Error al obtener el cliente por usuario", ex);
            }
        }

        public void ModificarCliente(int id, Clientes cliente)
        {
            try
            {
                var query = "UPDATE Clientes SET Nombre = @nombre, Email = @email, Telefono = @telefono WHERE ClienteId = @ClienteId";
                using (var connection = new SqliteConnection(cadenaConexion))
                {
                    connection.Open();
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ClienteId", id);
                        command.Parameters.AddWithValue("@nombre", cliente.Nombre);
                        command.Parameters.AddWithValue("@email", cliente.Email);
                        command.Parameters.AddWithValue("@telefono", cliente.Telefono);
                        int filasAfectadas = command.ExecuteNonQuery();

                        if (filasAfectadas == 0)
                        {
                            throw new Exception("No se encontró el cliente para modificar.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString()); // Logueo del error
                throw new Exception("Error al modificar el cliente", ex);
            }
        }

        public List<Clientes> ListarClientes()
        {
            try
            {
                var clientes = new List<Clientes>();
                var query = "SELECT * FROM clientes";
                using (var connection = new SqliteConnection(cadenaConexion))
                {
                    connection.Open();
                    using (var command = new SqliteCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var cliente = new Clientes(
                                    Convert.ToInt32(reader["ClienteId"]),
                                    reader["nombre"].ToString(),
                                    reader["email"].ToString(),
                                    reader["telefono"].ToString()
                                );
                                clientes.Add(cliente);
                            }
                        }
                    }
                }
                return clientes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString()); // Logueo del error
                throw new Exception("Error al listar los clientes", ex);
            }
        }

        public Clientes ObtenerCliente(int id)
        {
            try
            {
                Clientes cliente = null;
                var query = "SELECT * FROM clientes WHERE ClienteId = @ClienteId";
                using (var connection = new SqliteConnection(cadenaConexion))
                {
                    connection.Open();
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ClienteId", id);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                cliente = new Clientes(
                                    Convert.ToInt32(reader["ClienteId"]),
                                    reader["nombre"].ToString(),
                                    reader["email"].ToString(),
                                    reader["telefono"].ToString()
                                );
                            }
                            else
                            {
                                throw new Exception($"No se encontró un cliente con ID: {id}");
                            }
                        }
                    }
                }
                return cliente;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString()); // Logueo del error
                throw new Exception("Error al obtener el cliente", ex);
            }
        }

        public void EliminarCliente(int id)
        {
            try
            {
                var queryEliminarCliente = "DELETE FROM clientes WHERE ClienteId = @ClienteId";
                using (var connection = new SqliteConnection(cadenaConexion))
                {
                    connection.Open();
                    using (var command = new SqliteCommand(queryEliminarCliente, connection))
                    {
                        command.Parameters.AddWithValue("@ClienteId", id);
                        int filasAfectadas = command.ExecuteNonQuery();

                        if (filasAfectadas == 0)
                        {
                            throw new Exception($"No se encontró un cliente con ID: {id} para eliminar.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString()); // Logueo del error
                throw new Exception("Error al eliminar el cliente", ex);
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