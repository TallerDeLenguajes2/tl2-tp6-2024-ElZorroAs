using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging; // Importamos el espacio de nombres para el logger
using tl2_tp6_2024_ElZorroAs.Models;
using System;
using System.Collections.Generic;

namespace repositoriosTP6
{
    public class UsuariosRepository : IUsuariosRepository
    {
        private readonly string cadenaConexion;
        private readonly ILogger<UsuariosRepository> _logger; // Logger

        public UsuariosRepository(string cadenaDeConexion,ILogger<UsuariosRepository> logger)
        {
            cadenaConexion = cadenaDeConexion;
            _logger = logger; // Inicializamos el logger
        }
        public Usuarios ObtenerUsuario(string usuario, string contraseña)
        {
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(contraseña))
            {
                throw new ArgumentException("Usuario y contraseña no pueden estar vacíos.");
            }

            try
            {
                using (var conexion = new SqliteConnection(cadenaConexion))
                {
                    conexion.Open();
                    var comando = conexion.CreateCommand();
                    comando.CommandText = @"
                SELECT Id, Nombre, Usuario, Contraseña, Rol
                FROM Usuarios
                WHERE Usuario = @usuario";

                    comando.Parameters.AddWithValue("@usuario", usuario);

                    using (var lector = comando.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            string contraseñaAlmacenada = lector.GetString(lector.GetOrdinal("Contraseña"));

                            if (contraseñaAlmacenada != contraseña) // Aquí deberías usar hashing
                            {
                                _logger.LogWarning($"Intento de acceso inválido - Usuario: {usuario} Clave ingresada: {contraseña}");
                                return null;
                            }

                            _logger.LogInformation($"El usuario {usuario} ingresó correctamente."); // Logueo de acceso exitoso

                            return new Usuarios(
                                lector.GetInt32(lector.GetOrdinal("Id")),
                                lector.GetString(lector.GetOrdinal("Nombre")),
                                lector.GetString(lector.GetOrdinal("Usuario")),
                                lector.GetString(lector.GetOrdinal("Contraseña")),
                                lector.GetString(lector.GetOrdinal("Rol"))
                            );
                        }
                        else
                        {
                            throw new Exception($"No se encontró un usuario con el nombre: {usuario}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString()); // Logueo del error
                throw new Exception("Error al obtener el usuario", ex);
            }
        }

        public List<Usuarios> ListarUsuarios()
        {
            var usuarios = new List<Usuarios>();

            try
            {
                using (var conexion = new SqliteConnection(cadenaConexion))
                {
                    conexion.Open();
                    var comando = conexion.CreateCommand();
                    comando.CommandText = "SELECT Id, Nombre, Usuario, Contraseña, Rol FROM Usuarios";

                    using (var lector = comando.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            var usuario = new Usuarios(
                                lector.GetInt32(lector.GetOrdinal("Id")),
                                lector.GetString(lector.GetOrdinal("Nombre")),
                                lector.GetString(lector.GetOrdinal("Usuario")),
                                lector.GetString(lector.GetOrdinal("Contraseña")),
                                lector.GetString(lector.GetOrdinal("Rol"))
                            );

                            usuarios.Add(usuario);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString()); // Logueo del error
                throw new Exception("Error al listar los usuarios", ex);
            }

            return usuarios;
        }

        public Usuarios ObtenerUsuarioPorId(int id)
        {
            try
            {
                using (var conexion = new SqliteConnection(cadenaConexion))
                {
                    conexion.Open();
                    var comando = conexion.CreateCommand();
                    comando.CommandText = @"
                        SELECT Id, Nombre, Usuario, Contraseña, Rol
                        FROM Usuarios
                        WHERE Id = @id";

                    comando.Parameters.AddWithValue("@id", id);

                    using (var lector = comando.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            return new Usuarios(
                                lector.GetInt32(lector.GetOrdinal("Id")),
                                lector.GetString(lector.GetOrdinal("Nombre")),
                                lector.GetString(lector.GetOrdinal("Usuario")),
                                lector.GetString(lector.GetOrdinal("Contraseña")),
                                lector.GetString(lector.GetOrdinal("Rol"))
                            );
                        }
                        else
                        {
                            throw new Exception($"No se encontró un usuario con ID: {id}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString()); // Logueo del error
                throw new Exception("Error al obtener el usuario por ID", ex);
            }
        }

        public int? ObtenerIdClientePorUsuario(int idUsuario)
        {
            try
            {
                using (var connection = new SqliteConnection(cadenaConexion))
                {
                    connection.Open();
                    string query = "SELECT IdCliente FROM Usuarios WHERE Id = @idUsuario";

                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@idUsuario", idUsuario);
                        var result = command.ExecuteScalar();
                        if (result == null)
                        {
                            throw new Exception($"No se encontró un cliente asociado al usuario con ID: {idUsuario}");
                        }
                        return Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString()); // Logueo del error
                throw new Exception("Error al obtener el ID del cliente por usuario", ex);
            }
        }
    }
}