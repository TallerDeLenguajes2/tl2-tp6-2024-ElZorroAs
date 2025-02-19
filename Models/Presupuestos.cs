/*
*Presupuestos
○ int IdPresupuesto
○ string nombreDestinatario
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

    public Presupuestos(string nombreDestinatario, DateTime fechaCreacion, List<PresupuestosDetalle> detalle = null)
    {

        NombreDestinatario = nombreDestinatario;
        FechaCreacion = fechaCreacion;  // Aquí se pasa la fecha al crear el presupuesto
        Detalle = detalle ?? new List<PresupuestosDetalle>();
    }
    public Presupuestos(int idPresupuesto, string nombreDestinatario, DateTime fechaCreacion, List<PresupuestosDetalle> detalle = null)
    {
        IdPresupuesto = idPresupuesto;
        NombreDestinatario = nombreDestinatario;
        FechaCreacion = fechaCreacion;  // Aquí se pasa la fecha al crear el presupuesto
        Detalle = detalle ?? new List<PresupuestosDetalle>();
    }

    public int IdPresupuesto { get; private set; }

    public string NombreDestinatario { get; private set; }

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
