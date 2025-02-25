using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using tl2_tp6_2024_ElZorroAs.Models;

namespace tl2_tp6_2024_ElZorroAs.ViewModels
{
    public class PresupuestoViewModel
{
    public int IdPresupuesto { get; set; } 

    [Required]
    public int ClienteId { get; set; }

    [Required]
    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    public List<Clientes> ClientesDisponibles { get; set; } = new List<Clientes>();

    public List<PresupuestosDetalle> Detalle { get; set; } = new List<PresupuestosDetalle>();
}

}
