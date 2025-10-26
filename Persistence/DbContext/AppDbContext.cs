using Domain.Entities;
using Domain.Entities.General;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace Kadosh_erp.Persistence
{
    public class AppDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // 🔗 DbSets
        public DbSet<Country> Countries => Set<Country>();
        public DbSet<Province> Provinces => Set<Province>();
        public DbSet<City> Cities => Set<City>();
        public DbSet<Parish> Parishes => Set<Parish>();
        public DbSet<Zone> Zones => Set<Zone>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 🛡️ Filtro global por IsDeleted
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.ClrType.IsSubclassOf(typeof(AuditoryEntity)))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var isDeletedProp = Expression.Property(parameter, nameof(AuditoryEntity.IsDeleted));
                    var condition = Expression.Equal(isDeletedProp, Expression.Constant(false));
                    var lambda = Expression.Lambda(condition, parameter);
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }

            // 🧩 Aplica todas las configuraciones Fluent del ensamblado
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        public override int SaveChanges()
        {
            ApplyAuditRules();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditRules();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyAuditRules()
        {
            var userCode = _httpContextAccessor.HttpContext?.Request.Headers["userCode"].FirstOrDefault() ?? "system";

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is not AuditoryEntity entity) continue;

                switch (entry.State)
                {
                    case EntityState.Added:
                        entity.CreatedAt = DateTime.UtcNow;
                        entity.CreatedBy = userCode;
                        entity.Status = "Active";
                        entity.IsDeleted = false;
                        break;

                    case EntityState.Modified:
                        entity.ModifiedAt = DateTime.UtcNow;
                        entity.ModifiedBy = userCode;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entity.IsDeleted = true;
                        entity.Status = "Deleted";
                        entity.ModifiedAt = DateTime.UtcNow;
                        entity.ModifiedBy = userCode;
                        break;
                }
            }
        }
    }
}