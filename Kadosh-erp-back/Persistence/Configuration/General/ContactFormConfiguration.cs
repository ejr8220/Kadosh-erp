using Domain.Entities.General;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration.General
{
    public class ContactFormConfiguration : IEntityTypeConfiguration<ContactForm>
    {
        public void Configure(EntityTypeBuilder<ContactForm> builder)
        {
            builder.ToTable("ContactForms");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(80);

            builder.Property(x => x.Description)
                .HasMaxLength(200);

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

            builder.HasIndex(x => x.Name)
                .IsUnique();

            builder.HasMany(x => x.ContactFormPersons)
                .WithOne(x => x.ContactForm)
                .HasForeignKey(x => x.ContactFormId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
