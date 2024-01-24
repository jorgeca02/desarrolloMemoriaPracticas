using ApiLibros.Data;
using ApiLibros.Mapper;
using ApiLibros.Repositorio;
using ApiLibros.Repositorio.IRepositorio;
using ApiLibros.Servicio;
using ApiLibros.Servicio.IServicio;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

//Conexion base de datos sql server
builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
{
    opciones.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSql"));
});

//Se agregan los repositorios
builder.Services.AddScoped<ILibrosRepositorio, LibrosRepositorio>();

//Agregar el AutoMapper
builder.Services.AddAutoMapper(typeof(LibrosMapper));
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
var routeApiAutores = builder.Configuration.GetSection("ApiSettings")["routeApiAutores"];
var routeApiEditoriales = builder.Configuration.GetSection("ApiSettings")["routeApiEditoriales"];
var routeApiPersonas = builder.Configuration.GetSection("ApiSettings")["routeApiPersonas"];
var routeApiChats = builder.Configuration.GetSection("ApiSettings")["routeApiChat"];

// Add the Servicio with the API settings
builder.Services.AddScoped<IServicioLibros>(_ => new ServicioLibros(routeApiAutores,routeApiEditoriales,routeApiPersonas,routeApiChats));


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
