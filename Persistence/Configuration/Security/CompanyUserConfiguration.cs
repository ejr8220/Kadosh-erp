using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.Security;

namespace Persistence.Configuration.Security
{
    internal class CompanyUserConfiguration : IEntityTypeConfiguration<CompanyUser>
    {
        public void Configure(EntityTypeBuilder<CompanyUser> builder)
        {
            builder.ToTable("CompanyUsers");

            // 🔐 Clave primaria compuesta
            builder.HasKey(cu => new { cu.CompanyId, cu.UserId });

            // 🔗 Relaciones
            builder.HasOne(cu => cu.Company)
                .WithMany(c => c.CompanyUsers)
                .HasForeignKey(cu => cu.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(cu => cu.User)
                .WithMany(u => u.CompanyUsers)
                .HasForeignKey(cu => cu.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // 🧩 Propiedades heredadas de AuditoryEntity
            builder.Property(cu => cu.CreatedAt).IsRequired();
            builder.Property(cu => cu.CreatedBy).HasMaxLength(30);
            builder.Property(cu => cu.ModifiedAt);
            builder.Property(cu => cu.ModifiedBy).HasMaxLength(30);
            builder.Property(cu => cu.IsDeleted).IsRequired();
            builder.Property(cu => cu.Status).IsRequired().HasMaxLength(20);
        }
    }
}