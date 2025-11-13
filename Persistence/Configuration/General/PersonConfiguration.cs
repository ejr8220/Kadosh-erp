using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.General;

namespace Persistence.Configuration.General;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("Persons");

        // Clave primaria
        builder.HasKey(p => p.Id);

        // Propiedades principales
        builder.Property(p => p.IdentificationTypeId)
            .IsRequired(); 
        
        builder.Property(p => p.IdentificationNumber)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(p => p.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.BirthDate);

        builder.Property(p => p.Gender)
            .HasMaxLength(10);

        builder.Property(p => p.CountryId)
            .IsRequired();

        builder.Property(p => p.ProvinceId)
            .IsRequired();

        builder.Property(p => p.CityId)
            .IsRequired();

        builder.Property(p => p.ParishId)
            .IsRequired();

        // Propiedades de auditoría / metadatos
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

        // Índices
        builder.HasIndex(p => p.IdentificationNumber).IsUnique();

        // Relaciones (con eliminación restrictiva)
        builder.HasOne(p => p.Country)
            .WithMany()
            .HasForeignKey(p => p.CountryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Province)
            .WithMany()
            .HasForeignKey(p => p.ProvinceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.City)
            .WithMany()
            .HasForeignKey(p => p.CityId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Parish)
            .WithMany()
            .HasForeignKey(p => p.ParishId)
            .OnDelete(DeleteBehavior.Restrict);
        builder
    .HasOne(p => p.IdentificationType)
    .WithMany() // o .WithMany(x => x.People) si tienes colección en IdentificationType
    .HasForeignKey(p => p.IdentificationTypeId);

    }
}