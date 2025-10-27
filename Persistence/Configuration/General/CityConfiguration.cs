using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.General;

namespace Persistence.Configurations.General;

public class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.ToTable("Cities");

        // 🔐 Clave primaria
        builder.HasKey(c => c.Id);

        // 🧩 Propiedades
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.ProvinceId)
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.CreatedBy)
            .HasMaxLength(30);

        builder.Property(c => c.ModifiedAt);

        builder.Property(c => c.ModifiedBy)
            .HasMaxLength(30);

        builder.Property(c => c.IsDeleted)
            .IsRequired();

        builder.Property(c => c.Status)
            .IsRequired()
            .HasMaxLength(20);

        // 🔐 Índice único (Name + ProvinceId)
        builder.HasIndex(c => new { c.Name, c.ProvinceId }).IsUnique();

        // 🔗 Relación con Province
        builder.HasOne(c => c.Province)
            .WithMany(p => p.Cities)
            .HasForeignKey(c => c.ProvinceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}