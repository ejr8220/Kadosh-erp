using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Kadosh_erp.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;



namespace Persistence.DbContext
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var candidatePaths = new[]
            {
                currentDirectory,
                Path.Combine(currentDirectory, "Kadosh-erp"),
                Path.GetFullPath(Path.Combine(currentDirectory, "..", "Kadosh-erp"))
            };

            var basePath = candidatePaths.FirstOrDefault(path => File.Exists(Path.Combine(path, "appsettings.json")));
            if (basePath is null)
            {
                throw new FileNotFoundException("No se encontró appsettings.json en las rutas esperadas para migraciones.");
            }

            // 🛡️ Carga configuración desde appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connectionString = configuration.GetConnectionString("Default");

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            // 🧪 Simula IHttpContextAccessor para migraciones
            var fakeHttpContextAccessor = new HttpContextAccessor
            {
                HttpContext = new DefaultHttpContext()
            };

            return new AppDbContext(optionsBuilder.Options, fakeHttpContextAccessor);
        }
    }
}