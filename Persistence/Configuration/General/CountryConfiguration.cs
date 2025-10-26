using Domain.Entities.General;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Persistence.Configuration.General
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.ToTable("Countries");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.IsoCode)
                .HasMaxLength(10);

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

            builder.HasMany(c => c.Provinces)
                .WithOne(p => p.Country)
                .HasForeignKey(p => p.CountryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }    
}
