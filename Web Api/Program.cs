using Microsoft.EntityFrameworkCore;
using Web_Api.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Conexión a SQL Server (la cadena está en appsettings.json -> ConnectionStrings:DefaultConnection)
builder.Services.AddDbContext<DbContextSQLSRV>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// La ruta "/" ya la responde HomeController; tenerla dos veces provoca error al abrirla.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
