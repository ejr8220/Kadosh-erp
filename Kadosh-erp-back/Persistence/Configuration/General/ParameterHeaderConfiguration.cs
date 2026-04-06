using Domain.Entities.General;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration.General
{
    public class ParameterHeaderConfiguration : IEntityTypeConfiguration<ParameterHeader>
    {
        public void Configure(EntityTypeBuilder<ParameterHeader> builder)
        {
            builder.ToTable("ParameterHeaders");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(80);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(120);

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.Property(x => x.Scope)
                .IsRequired();

            builder.HasIndex(x => x.Code).IsUnique();

            builder.HasMany(x => x.Details)
                .WithOne(x => x.ParameterHeader)
                .HasForeignKey(x => x.ParameterHeaderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
