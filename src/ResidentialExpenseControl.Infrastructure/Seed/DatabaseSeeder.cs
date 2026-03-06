using Microsoft.EntityFrameworkCore;
using ResidentialExpenseControl.Domain.Entities;
using ResidentialExpenseControl.Domain.Enums;
using ResidentialExpenseControl.Infrastructure.Context;

namespace ResidentialExpenseControl.Infrastructure.Seed
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(ResidentialExpenseControlContext db)
        {
            // =========================
            // 1) CATEGORIES
            // =========================
            if (!await db.Categories.AnyAsync())
            {
                db.Categories.AddRange(
                    // Expense
                    new Category { Description = "Alimentação", Purpose = CategoryPurpose.Expense },
                    new Category { Description = "Transporte", Purpose = CategoryPurpose.Expense },
                    new Category { Description = "Saúde", Purpose = CategoryPurpose.Expense },
                    new Category { Description = "Educação", Purpose = CategoryPurpose.Expense },
                    new Category { Description = "Lazer", Purpose = CategoryPurpose.Expense },
                    new Category { Description = "Moradia", Purpose = CategoryPurpose.Expense },
                    new Category { Description = "Contas", Purpose = CategoryPurpose.Expense },
                    new Category { Description = "Assinaturas", Purpose = CategoryPurpose.Expense },

                    // Income
                    new Category { Description = "Salário", Purpose = CategoryPurpose.Income },
                    new Category { Description = "Freelance", Purpose = CategoryPurpose.Income },
                    new Category { Description = "Bônus", Purpose = CategoryPurpose.Income },
                    new Category { Description = "Reembolso", Purpose = CategoryPurpose.Income },

                    // Both (Ambas)
                    new Category { Description = "Investimentos", Purpose = CategoryPurpose.Both },
                    new Category { Description = "Transferências", Purpose = CategoryPurpose.Both },
                    new Category { Description = "Outros", Purpose = CategoryPurpose.Both }
                );

                await db.SaveChangesAsync();
            }

            // =========================
            // 2) PEOPLE
            // =========================
            if (!await db.People.AnyAsync())
            {
                db.People.AddRange(
                    new Person { Name = "João", Age = 30 },
                    new Person { Name = "Maria", Age = 16 },
                    new Person { Name = "Ana", Age = 22 },
                    new Person { Name = "Carlos", Age = 17 },
                    new Person { Name = "Fernanda", Age = 28 },
                    new Person { Name = "Pedro", Age = 19 },
                    new Person { Name = "Beatriz", Age = 14 },
                    new Person { Name = "Lucas", Age = 33 },
                    new Person { Name = "Mariana", Age = 21 },
                    new Person { Name = "Ricardo", Age = 36 },
                    new Person { Name = "Paulo", Age = 45 },
                    new Person { Name = "Tatiane", Age = 30 },
                    new Person { Name = "Gustavo", Age = 18 },
                    new Person { Name = "Helena", Age = 15 },
                    new Person { Name = "Rafaela", Age = 26 },
                    new Person { Name = "Bruno", Age = 40 },
                    new Person { Name = "Sofia", Age = 13 }
                );

                await db.SaveChangesAsync();
            }

            // =========================
            // 3) TRANSACTIONS 
            // =========================
            if (!await db.Transactions.AnyAsync())
            {
                // Carrega tudo pra lookup por nome/descrição
                var categories = await db.Categories.ToListAsync();
                var people = await db.People.ToListAsync();

                Category Cat(string desc) => categories.Single(c => c.Description == desc);
                Person PersonBy(string name) => people.Single(p => p.Name == name);

                // Helpers (só pra reduzir repetição)
                Transaction Exp(string desc, decimal amount, string catDesc, string personName) => new()
                {
                    Description = desc,
                    Amount = amount,
                    Type = TransactionType.Expense,
                    CategoryId = Cat(catDesc).Id,
                    PersonId = PersonBy(personName).Id
                };

                Transaction Inc(string desc, decimal amount, string catDesc, string personName) => new()
                {
                    Description = desc,
                    Amount = amount,
                    Type = TransactionType.Income,
                    CategoryId = Cat(catDesc).Id,
                    PersonId = PersonBy(personName).Id
                };

                db.Transactions.AddRange(
                    Inc("Salário - Março", 5200.00m, "Salário", "João"),
                    Exp("Supermercado", 430.75m, "Alimentação", "João"),
                    Exp("Gasolina", 220.50m, "Transporte", "João"),
                    Exp("Assinatura Streaming", 39.90m, "Assinaturas", "João"),

                    Exp("Lanche na escola", 18.50m, "Alimentação", "Maria"),
                    Exp("Cinema", 45.00m, "Lazer", "Maria"),

                    Inc("Freelance - Landing Page", 850.00m, "Freelance", "Ana"),
                    Exp("Farmácia", 62.30m, "Saúde", "Ana"),

                    Exp("Material escolar", 120.00m, "Educação", "Carlos"),
                    Exp("Ônibus (passe)", 90.00m, "Transporte", "Carlos"),

                    Inc("Salário - Março", 4200.00m, "Salário", "Fernanda"),
                    Exp("Aluguel", 1600.00m, "Moradia", "Fernanda"),
                    Exp("Internet", 119.90m, "Contas", "Fernanda"),

                    Inc("Bônus trimestral", 600.00m, "Bônus", "Pedro"),
                    Exp("Restaurante", 95.00m, "Alimentação", "Pedro"),

                    Exp("Sorvete", 12.00m, "Alimentação", "Beatriz"),
                    Exp("Jogo (promoção)", 29.90m, "Lazer", "Beatriz"),

                    Inc("Salário - Março", 7800.00m, "Salário", "Lucas"),
                    Exp("IPTU (parcela)", 320.00m, "Moradia", "Lucas"),
                    Exp("Academia", 89.90m, "Saúde", "Lucas"),

                    Inc("Reembolso Uber", 55.00m, "Reembolso", "Mariana"),
                    Exp("Uber", 48.00m, "Transporte", "Mariana"),

                    Inc("Freelance - API", 1300.00m, "Freelance", "Ricardo"),
                    Exp("Energia elétrica", 210.40m, "Contas", "Ricardo"),
                    Exp("Mercado", 510.10m, "Alimentação", "Ricardo"),

                    Inc("Salário - Março", 9500.00m, "Salário", "Paulo"),
                    Exp("Plano de saúde", 680.00m, "Saúde", "Paulo"),
                    Exp("Combustível", 350.00m, "Transporte", "Paulo"),

                    Inc("Bônus anual", 2200.25m, "Bônus", "Tatiane"),
                    Exp("Assinatura música", 21.90m, "Assinaturas", "Tatiane"),

                    Inc("Salário - Estágio", 1500.00m, "Salário", "Gustavo"),
                    Exp("Curso online", 79.90m, "Educação", "Gustavo"),

                    Exp("Livro", 35.00m, "Educação", "Helena"),
                    Exp("Lanche", 14.90m, "Alimentação", "Helena"),

                    Inc("Rendimento investimento", 250.00m, "Investimentos", "Rafaela"),
                    Exp("Manutenção bike", 110.00m, "Outros", "Rafaela"),

                    Inc("Salário - Março", 6100.00m, "Salário", "Bruno"),
                    Exp("Aluguel", 1800.00m, "Moradia", "Bruno"),

                    Exp("Passeio escolar", 85.00m, "Lazer", "Sofia"),
                    Exp("Transporte (van)", 70.00m, "Transporte", "Sofia")
                );

                await db.SaveChangesAsync();
            }
        }
    }
}
