using Domain.Entities;
using Domain.Entities.General;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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

        public DbSet<Country> Countries => Set<Country>();
        public DbSet<Province> Provinces => Set<Province>();
        public DbSet<City> Cities => Set<City>();
        public DbSet<Parish> Parishes => Set<Parish>();
        public DbSet<Zone> Zones => Set<Zone>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var auditoryBaseType = typeof(AuditoryEntity);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var clrType = entityType.ClrType;

                if (!auditoryBaseType.IsAssignableFrom(clrType) || clrType == auditoryBaseType)
                    continue;

                var method = typeof(AppDbContext)
                    .GetMethod(nameof(SetQueryFilter), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                    ?.MakeGenericMethod(clrType);

                method?.Invoke(null, new object[] { modelBuilder });
            }

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        private static void SetQueryFilter<TEntity>(ModelBuilder builder) where TEntity : AuditoryEntity
        {
            builder.Entity<TEntity>().HasQueryFilter(e => !e.IsDeleted);
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