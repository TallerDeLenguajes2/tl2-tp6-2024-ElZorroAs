using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging; // Importamos el espacio de nombres para el logger
using tl2_tp6_2024_ElZorroAs.Models;

namespace repositoriosTP6
{

    public class PresupuestosRepository : IPresupuestoRepository

    {
        private readonly string cadenaConexion;
        private readonly ILogger<PresupuestosRepository> _logger;
        private readonly IClientesRepository _clienteRepository;
        private readonly IProductoRepository _productoRepository;

        // Inyectamos el logger en el constructor
        public PresupuestosRepository(string cadenaDeConexion,ILogger<PresupuestosRepository> logger, IClientesRepository clienteRepository,IProductoRepository productoRepository)
        {
            cadenaConexion = cadenaDeConexion;
            _logger = logger;
            _clienteRepository = clienteRepository;
            _productoRepository = productoRepository;
        }
        public void CrearPresupuesto(Presupuestos presupuesto)
        {
            using (SqliteConnection connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string queryPresupuesto = "INSERT INTO Presupuestos (ClienteId, FechaCreacion) VALUES (@clienteId, @fechaCreacion)";
                        using (SqliteCommand command = new SqliteCommand(queryPresupuesto, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@clienteId", presupuesto.Cliente.ClienteId);
                            command.Parameters.AddWithValue("@fechaCreacion", presupuesto.FechaCreacion);
                            command.ExecuteNonQuery();
                        }

                        string queryIdPresupuesto = "SELECT last_insert_rowid()";
                        int idPresupuesto = 0;
                        using (SqliteCommand command = new SqliteCommand(queryIdPresupuesto, connection, transaction))
                        {
                            idPresupuesto = Convert.ToInt32(command.ExecuteScalar());
                        }

                        string queryDetalle = "INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@idPresupuesto, @idProducto, @Cantidad)";
                        using (SqliteCommand command = new SqliteCommand(queryDetalle, connection, transaction))
                        {
                            foreach (var detalle in presupuesto.Detalle)
                            {
                                command.Parameters.Clear();
                                command.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);
                                command.Parameters.AddWithValue("@idProducto", detalle.Producto.IdProducto);
                                command.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                                command.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _logger.LogError(ex, "Error al crear presupuesto");
                        throw new Exception($"Error al crear presupuesto: {ex.Message}");
                    }
                }
            }
        }

        public void AgregarProductoAPresupuesto(int presupuestoId, Productos producto, int cantidad)
        {
            try
            {
                using (var connection = new SqliteConnection(cadenaConexion))
                {
                    connection.Open();

                    // Verificar si el presupuesto existe
                    string queryCheckPresupuesto = "SELECT COUNT(*) FROM Presupuestos WHERE idPresupuesto = @idPresupuesto";
                    using (var commandCheckPresupuesto = new SqliteCommand(queryCheckPresupuesto, connection))
                    {
                        commandCheckPresupuesto.Parameters.AddWithValue("@idPresupuesto", presupuestoId);
                        int countPresupuesto = Convert.ToInt32(commandCheckPresupuesto.ExecuteScalar());

                        _logger.LogInformation("Verificando presupuesto con id {IdPresupuesto}, encontrado: {CountPresupuesto}", presupuestoId, countPresupuesto);

                        if (countPresupuesto == 0)
                        {
                            throw new Exception($"El presupuesto con ID {presupuestoId} no existe.");
                        }
                    }

                    // Insertar producto en el detalle del presupuesto
                    string query = "INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@idPresupuesto, @idProducto, @Cantidad)";
                    using (var command = new SqliteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@idPresupuesto", presupuestoId);
                        command.Parameters.AddWithValue("@idProducto", producto.IdProducto);
                        command.Parameters.AddWithValue("@Cantidad", cantidad);
                        command.ExecuteNonQuery();
                    }

                    _logger.LogInformation("Producto con id {IdProducto} agregado al presupuesto con id {IdPresupuesto} exitosamente", producto.IdProducto, presupuestoId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar producto al presupuesto con id {IdPresupuesto}", presupuestoId);
                throw new Exception($"Error al agregar producto al presupuesto: {ex.Message}");
            }
        }
        public void EliminarPresupuesto(int id)
        {
            using (var connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Primero eliminamos los detalles del presupuesto
                        string queryDetalle = "DELETE FROM PresupuestosDetalle WHERE idPresupuesto = @id";
                        using (var commandDetalle = new SqliteCommand(queryDetalle, connection, transaction))
                        {
                            commandDetalle.Parameters.AddWithValue("@id", id);
                            commandDetalle.ExecuteNonQuery();
                        }

                        // Luego eliminamos el presupuesto
                        string queryPresupuesto = "DELETE FROM Presupuestos WHERE idPresupuesto = @id";
                        using (var commandPresupuesto = new SqliteCommand(queryPresupuesto, connection, transaction))
                        {
                            commandPresupuesto.Parameters.AddWithValue("@id", id);
                            commandPresupuesto.ExecuteNonQuery();
                        }

                        // Confirmamos la transacción
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Si ocurre un error, revertimos la transacción
                        transaction.Rollback();
                        _logger.LogError(ex, "Error al eliminar el presupuesto con ID {IdPresupuesto}", id);
                        throw new Exception($"Error al eliminar el presupuesto: {ex.Message}");
                    }
                }
            }
        }

        public Presupuestos ObtenerPresupuesto(int id)
        {


            Clientes cliente = null;
            DateTime fechaCreacion = DateTime.MinValue;
            List<PresupuestosDetalle> detalles = new List<PresupuestosDetalle>();

            using (var connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();
                string query = @"SELECT idPresupuesto, ClienteId, FechaCreacion, idProducto, Cantidad 
                         FROM Presupuestos P
                         INNER JOIN PresupuestosDetalle PD USING(idPresupuesto)
                         WHERE P.idPresupuesto = @id";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (fechaCreacion == DateTime.MinValue)
                            {
                                int clienteId = reader.GetInt32(1);
                                cliente = _clienteRepository.ObtenerCliente(clienteId);
                                fechaCreacion = reader.GetDateTime(2);
                            }

                            var producto = _productoRepository.ObtenerProducto(reader.GetInt32(3));
                            int cantidad = reader.GetInt32(4);
                            detalles.Add(new PresupuestosDetalle(producto, cantidad));
                        }
                    }
                }
            }

            return new Presupuestos(id, cliente, fechaCreacion, detalles);
        }

        public void ModificarPresupuesto(Presupuestos presupuesto)
        {
            using (var connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction()) // 🔹 Se usa transacción para asegurar consistencia
                {
                    try
                    {
                        // 🔹 1️⃣ Actualizar el presupuesto (cliente y fecha)
                        string queryPresupuesto =
                            "UPDATE Presupuestos SET ClienteId = @ClienteId, FechaCreacion = @FechaCreacion WHERE IdPresupuesto = @IdPresupuesto";

                        using (var command = new SqliteCommand(queryPresupuesto, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@ClienteId", presupuesto.Cliente.ClienteId);
                            command.Parameters.AddWithValue("@FechaCreacion", presupuesto.FechaCreacion.ToString("yyyy-MM-dd"));
                            command.Parameters.AddWithValue("@IdPresupuesto", presupuesto.IdPresupuesto);
                            command.ExecuteNonQuery();
                        }

                        // 🔹 2️⃣ Eliminar detalles antiguos
                        string queryDeleteDetalle = "DELETE FROM PresupuestosDetalle WHERE IdPresupuesto = @IdPresupuesto";
                        using (var command = new SqliteCommand(queryDeleteDetalle, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@IdPresupuesto", presupuesto.IdPresupuesto);
                            command.ExecuteNonQuery();
                        }

                        // 🔹 3️⃣ Insertar nuevos detalles
                        string queryInsertDetalle =
                            "INSERT INTO PresupuestosDetalle (IdPresupuesto, IdProducto, Cantidad) VALUES (@IdPresupuesto, @IdProducto, @Cantidad)";

                        foreach (var detalle in presupuesto.Detalle)
                        {
                            using (var command = new SqliteCommand(queryInsertDetalle, connection, transaction))
                            {
                                command.Parameters.AddWithValue("@IdPresupuesto", presupuesto.IdPresupuesto);
                                command.Parameters.AddWithValue("@IdProducto", detalle.Producto.IdProducto);
                                command.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                                command.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit(); // 🔹 Guardar cambios en la base de datos
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback(); // 🔹 Deshacer cambios si hay error
                        throw new Exception("Error al modificar el presupuesto", ex);
                    }
                }
            }
        }

        public List<Presupuestos> ListarPresupuestos()
        {
            List<Presupuestos> listaPresupuestos = new List<Presupuestos>();



            using (var connection = new SqliteConnection(cadenaConexion))
            {
                connection.Open();
                string query = "SELECT idPresupuesto, ClienteId, FechaCreacion FROM Presupuestos";
                using (var command = new SqliteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int idPresupuesto = reader.GetInt32(0);
                            int clienteId = reader.GetInt32(1);
                            DateTime fechaCreacion = reader.GetDateTime(2);

                            var cliente = _clienteRepository.ObtenerCliente(clienteId);
                            List<PresupuestosDetalle> detalles = new List<PresupuestosDetalle>();

                            string queryDetalles = @"SELECT idProducto, Cantidad FROM PresupuestosDetalle WHERE idPresupuesto = @idPresupuesto";
                            var commandDetalles = new SqliteCommand(queryDetalles, connection);
                            commandDetalles.Parameters.AddWithValue("@idPresupuesto", idPresupuesto);
                            using (var readerDetalles = commandDetalles.ExecuteReader())
                            {
                                while (readerDetalles.Read())
                                {
                                    var producto = _productoRepository.ObtenerProducto(readerDetalles.GetInt32(0));
                                    int cantidad = readerDetalles.GetInt32(1);
                                    detalles.Add(new PresupuestosDetalle(producto, cantidad));
                                }
                            }

                            listaPresupuestos.Add(new Presupuestos(idPresupuesto, cliente, fechaCreacion, detalles));
                        }
                    }
                }
            }

            return listaPresupuestos;
        }

    }
}