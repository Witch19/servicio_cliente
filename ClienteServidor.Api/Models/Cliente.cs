using System.ComponentModel.DataAnnotations;

namespace ClientesServicios.Api.Models;

public class Cliente
{
    public int Id { get; set; }

    [Required, MaxLength(255)]
    public string NombreCliente { get; set; } = null!;

    [Required, MaxLength(255), EmailAddress]
    public string Correo { get; set; } = null!;
}
