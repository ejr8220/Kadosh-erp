using Application.Dtos.Request.General;
using Application.Dtos.Response.General;
using Application.Interfaces;
using Application.Mappings;
using Application.Services;
using Application.Services.General;
using Application.Validators.General;
using Domain.Entities.General;
using Domain.Interfaces;
using FluentValidation;
using Kadosh_erp.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 🔧 Servicios generales
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));


builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddAutoMapper(typeof(MaritalStatusProfile));

builder.Services.AddScoped<IGenericFilterService, GenericFilterService>();
builder.Services.AddScoped<ICrudService<MaritalStatusRequestDto, MaritalStatusResponseDto>, MaritalStatusService>();
// 🔧 Validaciones
builder.Services.AddValidatorsFromAssemblyContaining<ZoneDtoValidator>();

// 🔧 Controladores y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Kadosh ERP API", Version = "v1" });

});

var app = builder.Build();

// 🔧 Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Kadosh ERP API");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();