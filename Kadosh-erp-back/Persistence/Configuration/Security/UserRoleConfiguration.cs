using Domain.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration.Security
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRoles");

            // 🔐 Clave primaria
            builder.HasKey(ur => ur.Id);

            // 🧩 Propiedades
            builder.Property(ur => ur.UserId)
                .IsRequired();

            builder.Property(ur => ur.RoleId)
                .IsRequired();

            builder.Property(ur => ur.CreatedAt)
                .IsRequired();

            builder.Property(ur => ur.CreatedBy)
                .HasMaxLength(30);

            builder.Property(ur => ur.ModifiedAt);

            builder.Property(ur => ur.ModifiedBy)
                .HasMaxLength(30);

            builder.Property(ur => ur.IsDeleted)
                .IsRequired();

            builder.Property(ur => ur.Status)
                .IsRequired()
                .HasMaxLength(20);

            // 🔐 Índices únicos
            builder.HasIndex(ur => new { ur.UserId, ur.RoleId }).IsUnique();

            // 🔗 Relaciones
            builder.HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}