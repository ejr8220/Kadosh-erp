using Domain.Entities.General;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration.General
{
    public class ParameterDetailConfiguration : IEntityTypeConfiguration<ParameterDetail>
    {
        public void Configure(EntityTypeBuilder<ParameterDetail> builder)
        {
            builder.ToTable("ParameterDetails");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Value1)
                .IsRequired()
                .HasMaxLength(4000);

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.EffectiveFrom)
                .IsRequired();

            builder.HasIndex(x => new { x.ParameterHeaderId, x.CompanyId, x.IsActive });

            builder.HasOne(x => x.Company)
                .WithMany()
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
