using Domain.Entities.General;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration.General
{
    public class ContactFormPersonConfiguration : IEntityTypeConfiguration<ContactFormPerson>
    {
        public void Configure(EntityTypeBuilder<ContactFormPerson> builder)
        {
            builder.ToTable("ContactFormPersons");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.PersonId)
                .IsRequired();

            builder.Property(x => x.ContactFormId)
                .IsRequired();

            builder.Property(x => x.ContextType)
                .IsRequired();

            builder.Property(x => x.Value)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.IsPrimary)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.CreatedBy)
                .HasMaxLength(30);

            builder.Property(x => x.ModifiedAt);

            builder.Property(x => x.ModifiedBy)
                .HasMaxLength(30);

            builder.Property(x => x.IsDeleted)
                .IsRequired();

            builder.Property(x => x.Status)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasOne(x => x.Person)
                .WithMany(x => x.ContactFormPersons)
                .HasForeignKey(x => x.PersonId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ContactForm)
                .WithMany(x => x.ContactFormPersons)
                .HasForeignKey(x => x.ContactFormId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new { x.PersonId, x.ContactFormId, x.ContextType })
                .IsUnique();
        }
    }
}
