using ApiAutor.Repositorio;
using ApiAutores.Mapper;
using ApiAutores.Repositorio.IRepositorio;
using ApiDireccion.Servicio;
using ApiLibros.Data;
using ApiLibros.Servicio.IServicio;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

//Conexion base de datos sql server
builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
{
    opciones.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSql"));
});

//Se agregan los repositorios
builder.Services.AddScoped<IAutoresRepositorio, AutoresRepositorio>();

//Agregar el AutoMapper
builder.Services.AddAutoMapper(typeof(AutorMapper));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowConfiguredOrigins",
        corsBuilder =>
        {
            var corsOrigins = builder.Configuration.GetSection("CorsOrigins").Get<string[]>();
            corsBuilder.WithOrigins(corsOrigins)
                       .AllowAnyHeader()
                       .AllowAnyMethod();
        });
});
//Add memory cache
builder.Services.AddMemoryCache();

// Add services to the container.

// Obtain the API settings
var routeApiUsuarios = builder.Configuration.GetSection("ApiSettings")["routeApiUsuarios"];

// Add the Servicio with the API settings
builder.Services.AddScoped<IServicioLibros>(_ => new ServicioLibros(routeApiUsuarios));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}


app.UseHttpsRedirection();
app.UseCors("AllowConfiguredOrigins");
app.UseAuthorization();

app.MapControllers();

app.Run();
