using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.General;

namespace Persistence.Configurations.General;

public class ParishConfiguration : IEntityTypeConfiguration<Parish>
{
    public void Configure(EntityTypeBuilder<Parish> builder)
    {
        builder.ToTable("Parishes");

        // 🔐 Clave primaria
        builder.HasKey(p => p.Id);

        // 🧩 Propiedades
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.CityId)
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

        // 🔐 Índice único (Name + CityId)
        builder.HasIndex(p => new { p.Name, p.CityId }).IsUnique();

        // 🔗 Relación con City
        builder.HasOne(p => p.City)
            .WithMany(c => c.Parishes)
            .HasForeignKey(p => p.CityId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}