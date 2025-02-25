using System.ComponentModel.DataAnnotations;

namespace tl2_tp6_2024_ElZorroAs.Models;

public class Clientes
{
    public int ClienteId { get; private set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string Nombre { get; private set; }

    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress(ErrorMessage = "El email no tiene un formato válido.")]
    public string Email { get; private set; }

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [Phone(ErrorMessage = "El teléfono no tiene un formato válido.")]
    public string Telefono { get; private set; }

    public Clientes() { }

    // Constructor sin ID (para crear un nuevo cliente)
    public Clientes(string nombre, string email, string telefono)
    {
        Nombre = nombre;
        Email = email;
        Telefono = telefono;
    }

    // Constructor con ID (para recuperar un cliente existente)
    public Clientes(int clienteId, string nombre, string email, string telefono)
    {
        ClienteId = clienteId;
        Nombre = nombre;
        Email = email;
        Telefono = telefono;
    }
}
