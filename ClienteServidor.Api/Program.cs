using ClientesServicios.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// ---------- C O R S ----------
const string AllowAll = "_allowAll";
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(AllowAll, p =>
        p.AllowAnyOrigin()      // en dev: permite file://, 127.0.0.1, localhost, etc.
         .AllowAnyHeader()
         .AllowAnyMethod());
    // Si prefieres restringir: usa WithOrigins("http://localhost:5500","http://127.0.0.1:5500")
});

// ---------- DB ----------
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

// ---------- Controllers / Swagger ----------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// (Opcional) respuesta de validación uniforme 400
builder.Services.Configure<ApiBehaviorOptions>(o =>
{
    o.InvalidModelStateResponseFactory = ctx =>
    {
        var errors = ctx.ModelState.Where(x => x.Value?.Errors.Count > 0)
            .ToDictionary(k => k.Key, v => v.Value!.Errors.Select(e => e.ErrorMessage).ToArray());
        return new BadRequestObjectResult(new { message = "Validación fallida", errors });
    };
});

var app = builder.Build();

// ---------- Migraciones automáticas en dev ----------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// ---------- Swagger ----------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ---------- Archivos estáticos (wwwroot/index.html) ----------
app.UseDefaultFiles();
app.UseStaticFiles();

// ---------- CORS (antes de MapControllers) ----------
app.UseCors(AllowAll);

// ---------- Resto del pipeline ----------
app.UseAuthorization();
app.MapControllers();

// (Opcional) servir SPA por defecto: si no hay ruta API, devuelve index.html
// app.MapFallbackToFile("index.html");

app.Run();
