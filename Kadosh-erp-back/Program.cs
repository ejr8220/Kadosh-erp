using Application.Interfaces;
using Application.Mappings;
using Application.Services;
using Application.Services.General;
using Application.Services.Security;
using Application.Validators.General;
using Domain.Interfaces;
using FluentValidation;
using Infrastructure.Services.Security;
using Kadosh_erp.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
const string FrontCorsPolicy = "FrontCorsPolicy";

// 🔧 Servicios generales
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));


builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddAutoMapper(typeof(MaritalStatusProfile), typeof(AppMappingProfile));

builder.Services.AddScoped<IGenericFilterService, GenericFilterService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IParameterService, ParameterService>();
builder.Services.AddScoped<IPasswordHasher, Pbkdf2PasswordHasher>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();

// Registra automaticamente todas las implementaciones ICrudService<TRequest, TResponse>
var applicationAssembly = typeof(MaritalStatusService).Assembly;
var crudInterfaceType = typeof(ICrudService<,>);

foreach (var implementation in applicationAssembly.GetTypes()
             .Where(t => !t.IsAbstract && !t.IsInterface))
{
    var interfaces = implementation.GetInterfaces()
        .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == crudInterfaceType);

    foreach (var serviceInterface in interfaces)
    {
        builder.Services.AddScoped(serviceInterface, implementation);
    }
}
// 🔧 Validaciones
builder.Services.AddValidatorsFromAssemblyContaining<ZoneDtoValidator>();

var jwtKey = builder.Configuration["Jwt:Key"] ?? "CHANGE_ME_SUPER_SECRET_KEY_1234567890";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "KadoshERP";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "KadoshERP.Client";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(FrontCorsPolicy, policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

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
app.UseCors(FrontCorsPolicy);
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();