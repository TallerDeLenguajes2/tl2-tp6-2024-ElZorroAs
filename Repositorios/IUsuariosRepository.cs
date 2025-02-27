using tl2_tp6_2024_ElZorroAs.Models;
using System.Collections.Generic;

namespace repositoriosTP6
{
    public interface IUsuariosRepository
    {
        public Usuarios ObtenerUsuario(string usuario, string contraseña);
        public Usuarios ObtenerUsuarioPorId(int id);
        public List<Usuarios> ListarUsuarios();
        public int ? ObtenerIdClientePorUsuario(int idUsuario);
    }
}