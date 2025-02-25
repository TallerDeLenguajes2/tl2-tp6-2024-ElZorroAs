/*
*Presupuestos
○ int IdPresupuesto
○ Cliente Cliente
○ List<PresupuestoDetalle> detalle
○ Metodos
■ MontoPresupuesto ()
■ MontoPresupuestoConIva()
■ CantidadProductos ()
*/
using System;

namespace tl2_tp6_2024_ElZorroAs.Models;

public class Presupuestos
{

    public Presupuestos(Clientes cliente, DateTime fechaCreacion, List<PresupuestosDetalle> detalle = null)
    {

        Cliente = cliente;
        FechaCreacion = fechaCreacion;  // Aquí se pasa la fecha al crear el presupuesto
        Detalle = detalle ?? new List<PresupuestosDetalle>();
    }
    public Presupuestos(int idPresupuesto, Clientes cliente, DateTime fechaCreacion, List<PresupuestosDetalle> detalle = null)
    {
        IdPresupuesto = idPresupuesto;
        Cliente = cliente;
        FechaCreacion = fechaCreacion;  // Aquí se pasa la fecha al crear el presupuesto
        Detalle = detalle ?? new List<PresupuestosDetalle>();
    }

    public int IdPresupuesto { get; private set; }

    public Clientes Cliente { get; private set; }

    public List<PresupuestosDetalle> Detalle { get; private set; }

    public DateTime FechaCreacion { get; private set; } // Nueva propiedad

    public decimal MontoPresupuesto()
    {
        decimal monto = 0.0m;
        foreach (var item in Detalle)
        {
            monto += (item.Producto.Precio * item.Cantidad);
        }
        return monto;
    }


    public decimal MontoPresupuestoConIva()
    {
        const decimal IVA = 0.21m; // Definir IVA como decimal
        return MontoPresupuesto() * (1 + IVA);
    }

    public int CantidadProductos()
    {
        return Detalle.Count();
    }
}