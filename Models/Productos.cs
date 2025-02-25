/*Productos
○ int idProducto
○ string descripcion
○ int precio*/
using System;
using System.ComponentModel.DataAnnotations;

namespace tl2_tp6_2024_ElZorroAs.Models;

public class Productos
{
    public Productos(string descripcion, decimal precio)
    {
        Descripcion = descripcion;
        Precio = precio;
    }

    public Productos(int idProducto, string descripcion, decimal precio)
    {
        IdProducto = idProducto;
        Descripcion = descripcion;
        Precio = precio;
    }

    public int IdProducto { get; private set; }

    [StringLength(250, ErrorMessage = "La descripción no puede tener más de 250 caracteres.")]
    public string Descripcion { get; private set; }

    [Required(ErrorMessage = "El precio es obligatorio.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser un valor positivo.")]
    public decimal Precio { get; private set; }
}
