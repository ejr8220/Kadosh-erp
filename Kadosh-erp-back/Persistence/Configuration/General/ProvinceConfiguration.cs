using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.General;

namespace Persistence.Configurations.General;

public class ProvinceConfiguration : IEntityTypeConfiguration<Province>
{
    public void Configure(EntityTypeBuilder<Province> builder)
    {
        builder.ToTable("Provinces");

        // 🔐 Clave primaria
        builder.HasKey(p => p.Id);

        // 🧩 Propiedades
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.CountryId)
            .IsRequired();

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

        // 🔐 Índice único (Name + CountryId)
        builder.HasIndex(p => new { p.Name, p.CountryId }).IsUnique();

        // 🔗 Relación con Country
        builder.HasOne(p => p.Country)
            .WithMany(c => c.Provinces)
            .HasForeignKey(p => p.CountryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}