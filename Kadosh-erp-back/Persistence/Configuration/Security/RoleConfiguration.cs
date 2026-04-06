using Domain.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration.Security
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");

            // 🔐 Clave primaria
            builder.HasKey(r => r.Id);

            // 🧩 Propiedades
            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(r => r.Description)
                .HasMaxLength(250);

            builder.Property(r => r.CreatedAt)
                .IsRequired();

            builder.Property(r => r.CreatedBy)
                .HasMaxLength(30);

            builder.Property(r => r.ModifiedAt);

            builder.Property(r => r.ModifiedBy)
                .HasMaxLength(30);

            builder.Property(r => r.IsDeleted)
                .IsRequired();

            builder.Property(r => r.Status)
                .IsRequired()
                .HasMaxLength(20);

            // 🔐 Índice único
            builder.HasIndex(r => r.Name).IsUnique();

            // 🔗 Relaciones
            builder.HasMany(r => r.UserRoles)
                .WithOne()
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(r => r.RolePermissions)
                .WithOne()
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}