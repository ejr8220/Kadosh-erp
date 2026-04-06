using Domain.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration.General
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Companies");

            // 🔐 Clave primaria
            builder.HasKey(c => c.Id);

            // 🧩 Propiedades
            builder.Property(c => c.PersonId)
                .IsRequired();

            builder.Property(c => c.LegalpresentativeIdentification)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(c => c.LegalRepresentative)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.AccountantIdentification)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(c => c.Accountant)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.CompanyTypeId)
                .IsRequired()
                .HasDefaultValue(1);

            builder.Property(c => c.LogoUrl)
                .HasMaxLength(250);

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

            // 🔗 Relación 1:1 con Person evitando FK sombra (PersonId1)
            builder.HasOne(c => c.Person)
                .WithOne(p => p.Company)
                .HasForeignKey<Company>(c => c.PersonId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.CompanyType)
                .WithMany()
                .HasForeignKey(c => c.CompanyTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}