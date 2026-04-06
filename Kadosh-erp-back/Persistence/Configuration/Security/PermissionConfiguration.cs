using Domain.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration.Security
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permissions");

            // 🔐 Clave primaria
            builder.HasKey(p => p.Id);

            // 🧩 Propiedades
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Description)
                .HasMaxLength(250);

            builder.Property(p => p.CreatedAt)
                .IsRequired();

            builder.Property(p => p.CreatedBy)
                .HasMaxLength(30);

            builder.Property(p => p.ModifiedAt);

            builder.Property(p => p.ModifiedBy)
                .HasMaxLength(30);

            builder.Property(p => p.IsDeleted)
                .IsRequired();

            builder.Property(p => p.Status)
                .IsRequired()
                .HasMaxLength(20);

            // 🔐 Índice único
            builder.HasIndex(p => p.Name).IsUnique();

            // 🔗 Relaciones
            builder.HasMany(p => p.RolePermissions)
                .WithOne()
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}