using ClientesServicios.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ClientesServicios.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Servicio> Servicios => Set<Servicio>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Unique en correo
        modelBuilder.Entity<Cliente>()
            .HasIndex(c => c.Correo)
            .IsUnique();
    }
}
