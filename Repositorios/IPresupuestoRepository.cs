using tl2_tp6_2024_ElZorroAs.Models;
using System.Collections.Generic;

namespace rapositoriosTP5
{
    public interface IPresupuestoRepository
    {
        public void CrearPresupuesto(Presupuestos presupuesto);
        public List<Presupuestos> ListarPresupuestos();
        public Presupuestos ObtenerPresupuesto(int id);
        public void AgregarProductoAPresupuesto(int presupuestoId, Productos producto, int cantidad);
        public void EliminarPresupuesto(int id);
    }
}