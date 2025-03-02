using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging; // Importamos el espacio de nombres para el logger
using tl2_tp6_2024_ElZorroAs.Models;

namespace repositoriosTP6
{
    public class ClientesRepository : IClientesRepository
    {
        private string cadenaConexion = "Data Source=DB/tienda.db;Cache=Shared";

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
                throw new Exception("Error al crear el cliente", ex);
            }
        }

        public Clientes ObtenerClientePorUsuario(string usuarioNombre)
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
                }
            }
        }
        connection.Close();
    }
    return cliente;
}



        public void ModificarCliente(int id, Clientes cliente)
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

        public List<Clientes> ListarClientes()
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
                connection.Close();
            }
            return clientes;
        }

        public Clientes ObtenerCliente(int id)
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
                    }
                }
                connection.Close();
            }
            return cliente;
        }

        public void EliminarCliente(int id)
        {
            var queryEliminarCliente = "DELETE FROM clientes WHERE ClienteId = @ClienteId";
            using (var connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();
                using (var command = new SqliteCommand(queryEliminarCliente, connection))
                {
                    command.Parameters.AddWithValue("@ClienteId", id);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
    }
}
