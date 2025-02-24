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

    public double MontoPresupuesto()
    {
        double monto = 0.0;
        foreach (var item in Detalle)
        {
            monto += (item.Producto.Precio * item.Cantidad);
        }
        return monto;
    }

    public double MontoPresupuestoConIva()
    {
        const double IVA = 0.21;
        return MontoPresupuesto() * (1 + IVA);
    }

    public int CantidadProductos()
    {
        return Detalle.Count();
    }
}
