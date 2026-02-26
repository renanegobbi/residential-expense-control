using ResidentialExpenseControl.Domain.Entities;
using ResidentialExpenseControl.Domain.Enums;
using ResidentialExpenseControl.Infrastructure.Context;

namespace ResidentialExpenseControl.Infrastructure.Seed
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(ResidentialExpenseControlContext db)
        {
            if (!db.Categories.Any())
            {
                db.Categories.AddRange(
                    new Category { Description = "Alimentação", Purpose = CategoryPurpose.Expense },
                    new Category { Description = "Salário", Purpose = CategoryPurpose.Income },
                    new Category { Description = "Lazer", Purpose = CategoryPurpose.Expense }
                );

                await db.SaveChangesAsync();
            }

            if (!db.People.Any())
            {
                db.People.AddRange(
                    new Person { Name = "João", Age = 30 },
                    new Person { Name = "Maria", Age = 16 }
                );

                await db.SaveChangesAsync();
            }
        }
    }
}
