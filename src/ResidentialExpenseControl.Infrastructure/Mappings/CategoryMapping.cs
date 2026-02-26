using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ResidentialExpenseControl.Domain.Entities;

namespace ResidentialExpenseControl.Infrastructure.Mappings
{
    public class CategoryMapping : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Description)
                .IsRequired()
                .HasColumnType("TEXT")
                .HasMaxLength(400);

            builder.Property(c => c.Purpose)
                .IsRequired();

            builder.ToTable("Categories");
        }
    }
}
