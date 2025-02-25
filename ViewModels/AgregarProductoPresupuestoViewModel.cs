using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using tl2_tp6_2024_ElZorroAs.Models;

namespace tl2_tp6_2024_ElZorroAs.ViewModels
{
    public class AgregarProductoPresupuestoViewModel
    {
        [Required]
        public int IdPresupuesto { get; set; }

        [Required]
        public int ProductoId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Debe agregar al menos un producto.")]
        public int Cantidad { get; set; }

        public List<Productos> ProductosDisponibles { get; set; } = new List<Productos>();
    }
}
