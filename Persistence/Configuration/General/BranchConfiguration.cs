using Domain.Entities.General;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration.General
{
    public class BranchConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.ToTable("Branches");

            // 🔐 Clave primaria
            builder.HasKey(b => b.Id);

            // 🧩 Propiedades
            builder.Property(b => b.Code)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(b => b.Address)
                .HasMaxLength(200);

            builder.Property(b => b.Phone)
                .HasMaxLength(20);

            builder.Property(b => b.Email)
                .HasMaxLength(100);

            builder.Property(b => b.CompanyId)
                .IsRequired();

            builder.Property(b => b.CreatedAt)
                .IsRequired();

            builder.Property(b => b.CreatedBy)
                .HasMaxLength(30);

            builder.Property(b => b.ModifiedAt);

            builder.Property(b => b.ModifiedBy)
                .HasMaxLength(30);

            builder.Property(b => b.IsDeleted)
                .IsRequired();

            builder.Property(b => b.Status)
                .IsRequired()
                .HasMaxLength(20);

            // 🔐 Índice único
            builder.HasIndex(b => new { b.Code, b.CompanyId }).IsUnique();

            // 🔗 Relación con Company
            builder.HasOne(b => b.Company)
                .WithMany()
                .HasForeignKey(b => b.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}