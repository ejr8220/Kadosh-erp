using Domain.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration.Security
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            // 🔐 Clave primaria
            builder.HasKey(u => u.Id);

            // 🧩 Propiedades
            builder.Property(u => u.UserCode)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.CreatedAt)
                .IsRequired();

            builder.Property(u => u.CreatedBy)
                .HasMaxLength(30);

            builder.Property(u => u.ModifiedAt);

            builder.Property(u => u.ModifiedBy)
                .HasMaxLength(30);

            builder.Property(u => u.IsDeleted)
                .IsRequired();

            builder.Property(u => u.Status)
                .IsRequired()
                .HasMaxLength(20);

            // 🔐 Índices únicos
            builder.HasIndex(u => u.UserCode).IsUnique();
            builder.HasIndex(u => u.Email).IsUnique();

            // 🔗 Relaciones
            builder.HasMany(u => u.UserRoles)
                .WithOne()
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(u => u.CompanyUsers)
                .WithOne()
                .HasForeignKey(cu => cu.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}