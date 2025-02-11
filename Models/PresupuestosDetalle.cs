/*
PresupuestosDetalle
○ Productos producto
○ int cantidad
*/
using System;

namespace tl2_tp6_2024_ElZorroAs.Models;

public class PresupuestosDetalle
{
    
    public PresupuestosDetalle(Productos producto, int cantidad)
    {
        Producto = producto;
        Cantidad = cantidad;
    }

    public Productos Producto {get;private set;}
   
    public int Cantidad{get;private set;}
}