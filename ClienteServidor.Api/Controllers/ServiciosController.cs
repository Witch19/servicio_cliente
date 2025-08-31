using ClientesServicios.Api.Data;
using ClientesServicios.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClientesServicios.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServiciosController(AppDbContext db) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Servicio>> Create(Servicio servicio)
    {
        db.Servicios.Add(servicio);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = servicio.Id }, servicio);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Servicio>>> GetAll()
        => await db.Servicios.ToListAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Servicio>> GetById(int id)
    {
        var servicio = await db.Servicios.FindAsync(id);
        return servicio is null ? NotFound() : Ok(servicio);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Servicio update)
    {
        var servicio = await db.Servicios.FindAsync(id);
        if (servicio is null) return NotFound();

        servicio.NombreServicio = update.NombreServicio;
        servicio.Descripcion = update.Descripcion;
        await db.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var servicio = await db.Servicios.FindAsync(id);
        if (servicio is null) return NotFound();

        db.Servicios.Remove(servicio);
        await db.SaveChangesAsync();

        return NoContent();
    }
}
