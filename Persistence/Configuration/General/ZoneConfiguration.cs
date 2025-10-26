using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.General;

namespace Persistence.Configurations.General;

public class ZoneConfiguration : IEntityTypeConfiguration<Zone>
{
    public void Configure(EntityTypeBuilder<Zone> builder)
    {
        builder.ToTable("Zones");

        // 🔐 Clave primaria
        builder.HasKey(z => z.Id);

        // 🧩 Propiedades
        builder.Property(z => z.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(z => z.ParishId)
            .IsRequired();

        builder.Property(z => z.CreatedAt)
            .IsRequired();

        builder.Property(z => z.CreatedBy)
            .HasMaxLength(30);

        builder.Property(z => z.ModifiedAt);

        builder.Property(z => z.ModifiedBy)
            .HasMaxLength(30);

        builder.Property(z => z.IsDeleted)
            .IsRequired();

        builder.Property(z => z.Status)
            .IsRequired()
            .HasMaxLength(20);

        // 🔐 Índices únicos
        builder.HasIndex(z => new { z.Name, z.ParishId }).IsUnique();

        // 🔗 Relación con Parish
        builder.HasOne(z => z.Parish)
            .WithMany(p => p.Zones)
            .HasForeignKey(z => z.ParishId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}