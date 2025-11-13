using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.General;

namespace Persistence.Configuration.General
{
    public class MaritalStatusConfiguration : IEntityTypeConfiguration<MaritalStatus>
    {
        public void Configure(EntityTypeBuilder<MaritalStatus> builder)
        {
            builder.ToTable("MaritalStatuses");

            // Clave primaria
            builder.HasKey(m => m.Id);

            // Propiedades
            builder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(m => m.CreatedAt)
                .IsRequired();

            builder.Property(m => m.CreatedBy)
                .HasMaxLength(30);

            builder.Property(m => m.ModifiedAt);

            builder.Property(m => m.ModifiedBy)
                .HasMaxLength(30);

            builder.Property(m => m.IsDeleted)
                .IsRequired();

            builder.Property(m => m.Status)
                .IsRequired()
                .HasMaxLength(20);

            // Índice único sobre Name
            builder.HasIndex(m => m.Name).IsUnique();
        }
    }
}