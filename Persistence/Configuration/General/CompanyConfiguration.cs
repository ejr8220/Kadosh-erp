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

            // 🔗 Relación con Person
            builder.HasOne(c => c.Person)
                .WithMany()
                .HasForeignKey(c => c.PersonId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}