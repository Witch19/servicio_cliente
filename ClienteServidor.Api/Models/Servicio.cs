using System.ComponentModel.DataAnnotations;

namespace ClientesServicios.Api.Models;

public class Servicio
{
    public int Id { get; set; }

    [Required, MaxLength(255)]
    public string NombreServicio { get; set; } = null!;

    [MaxLength(255)]
    public string? Descripcion { get; set; }
}
