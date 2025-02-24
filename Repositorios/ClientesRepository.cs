using tl2_tp6_2024_ElZorroAs.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace repositoriosTP6
{
    public class ClienteRepository : IClientesRepository
    {
        private string cadenaConexion = "Data Source=DB/tienda.db;Cache=Shared";

        public void CrearCliente(Clientes cliente)
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
                connection.Close();
            }
        }

        public void ModificarCliente(int id, Clientes cliente)
        {
            var query = "UPDATE clientes SET nombre = @nombre, email = @email, telefono = @telefono WHERE ClienteId = @ClienteId";
            using (var connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();
                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClienteId", id);
                    command.Parameters.AddWithValue("@nombre", cliente.Nombre);
                    command.Parameters.AddWithValue("@email", cliente.Email);
                    command.Parameters.AddWithValue("@telefono", cliente.Telefono);
                    command.ExecuteNonQuery();
                }
                connection.Close();
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
