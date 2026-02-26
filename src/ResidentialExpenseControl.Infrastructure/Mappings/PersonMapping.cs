using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResidentialExpenseControl.Domain.Entities;

namespace ResidentialExpenseControl.Infrastructure.Mappings
{
    public class PersonMapping : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasColumnType("TEXT")
                .HasMaxLength(200);

            builder.Property(p => p.Age)
                .IsRequired();

            builder.ToTable("People");
        }
    }
}
