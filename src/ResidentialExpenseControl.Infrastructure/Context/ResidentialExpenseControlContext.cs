using Microsoft.EntityFrameworkCore;
using ResidentialExpenseControl.Domain.Entities;

namespace ResidentialExpenseControl.Infrastructure.Context
{
    public class ResidentialExpenseControlContext : DbContext
    {
        public ResidentialExpenseControlContext(DbContextOptions<ResidentialExpenseControlContext> options)
            : base(options)
        {
        }

        public DbSet<Person> People { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Applies IEntityTypeConfiguration<T> from this assembly (Mappings folder)
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ResidentialExpenseControlContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
