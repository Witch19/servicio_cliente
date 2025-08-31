using ClientesServicios.Api.Data;
using ClientesServicios.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClientesServicios.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController(AppDbContext db) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Cliente>> Create(Cliente cliente)
    {
        db.Clientes.Add(cliente);
        await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = cliente.Id }, cliente);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cliente>>> GetAll()
        => await db.Clientes.ToListAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Cliente>> GetById(int id)
    {
        var cliente = await db.Clientes.FindAsync(id);
        return cliente is null ? NotFound() : Ok(cliente);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Cliente update)
    {
        var cliente = await db.Clientes.FindAsync(id);
        if (cliente is null) return NotFound();

        cliente.NombreCliente = update.NombreCliente;
        cliente.Correo = update.Correo;
        await db.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var cliente = await db.Clientes.FindAsync(id);
        if (cliente is null) return NotFound();

        db.Clientes.Remove(cliente);
        await db.SaveChangesAsync();

        return NoContent();
    }
}
