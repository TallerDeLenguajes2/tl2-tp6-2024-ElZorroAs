using System.Collections.Generic;
using tl2_tp6_2024_ElZorroAs.Models;

namespace repositoriosTP6
{
    public interface IClientesRepository
    {
        void CrearCliente(Clientes cliente);
        void ModificarCliente(int id, Clientes cliente);
        List<Clientes> ListarClientes();
        Clientes ObtenerCliente(int id);
        void EliminarCliente(int id);
    }
}
