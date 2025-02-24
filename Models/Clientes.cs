namespace tl2_tp6_2024_ElZorroAs.Models;

public class Clientes
{
    public int ClienteId { get; private set; }
    public string Nombre { get; private set; }
    public string Email { get; private set; }
    public string Telefono { get; private set; }

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