using Domain.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration.Security
{
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.ToTable("RolePermissions");

            // 🔐 Clave primaria
            builder.HasKey(rp => rp.Id);

            // 🧩 Propiedades
            builder.Property(rp => rp.RoleId)
                .IsRequired();

            builder.Property(rp => rp.PermissionId)
                .IsRequired();

            builder.Property(rp => rp.CreatedAt)
                .IsRequired();

            builder.Property(rp => rp.CreatedBy)
                .HasMaxLength(30);

            builder.Property(rp => rp.ModifiedAt);

            builder.Property(rp => rp.ModifiedBy)
                .HasMaxLength(30);

            builder.Property(rp => rp.IsDeleted)
                .IsRequired();

            builder.Property(rp => rp.Status)
                .IsRequired()
                .HasMaxLength(20);

            // 🔐 Índices únicos
            builder.HasIndex(rp => new { rp.RoleId, rp.PermissionId }).IsUnique();

            // 🔗 Relaciones
            builder.HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}