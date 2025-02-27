using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using tl2_tp6_2024_ElZorroAs.Models;
using System.Collections.Generic;

namespace repositoriosTP6
{
    public class UsuariosRepository : IUsuariosRepository
    {
        private string cadenaConexion;

        public UsuariosRepository(IConfiguration configuracion)
        {
            cadenaConexion = "Data Source=DB/Tienda.db;Cache=Shared";
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
                        WHERE Usuario = @usuario AND Contraseña = @contraseña";

                    comando.Parameters.AddWithValue("@usuario", usuario);
                    comando.Parameters.AddWithValue("@contraseña", contraseña);

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
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el usuario", ex);
            }

            return null;
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
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el usuario por ID", ex);
            }
            return null;
        }

        public int? ObtenerIdClientePorUsuario(int idUsuario)
        {
            using (var connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();
                string query = "SELECT IdCliente FROM Usuarios WHERE Id = @idUsuario";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idUsuario", idUsuario);
                    var result = command.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : (int?)null;
                }
            }
        }
    }
}
