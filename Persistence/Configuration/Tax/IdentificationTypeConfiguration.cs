using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.Tax;

namespace Persistence.Configurations.Tax;

public class IdentificationTypeConfiguration : IEntityTypeConfiguration<IdentificationType>
{
    public void Configure(EntityTypeBuilder<IdentificationType> builder)
    {
        builder.ToTable("IdentificationTypes");

        // Clave primaria
        builder.HasKey(i => i.Id);

        // Propiedades
        builder.Property(i => i.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(i => i.Code)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(i => i.Maxlength)
            .IsRequired();

        // Campos de auditoría
        builder.Property(i => i.CreatedAt)
            .IsRequired();

        builder.Property(i => i.CreatedBy)
            .HasMaxLength(30);

        builder.Property(i => i.ModifiedAt);

        builder.Property(i => i.ModifiedBy)
            .HasMaxLength(30);

        builder.Property(i => i.IsDeleted)
            .IsRequired();

        builder.Property(i => i.Status)
            .IsRequired()
            .HasMaxLength(20);

        // Índices
        builder.HasIndex(i => i.Code).IsUnique();
    }
}