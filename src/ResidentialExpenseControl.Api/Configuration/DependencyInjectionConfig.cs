using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ResidentialExpenseControl.Core.Infrastructure;
using ResidentialExpenseControl.Domain.Interfaces;
using ResidentialExpenseControl.Domain.Interfaces.Infrastructure.Repositories;
using ResidentialExpenseControl.Domain.Interfaces.Services;
using ResidentialExpenseControl.Domain.Notifications;
using ResidentialExpenseControl.Domain.Services;
using ResidentialExpenseControl.Infrastructure;
using ResidentialExpenseControl.Infrastructure.Context;
using ResidentialExpenseControl.Infrastructure.Repositories;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ResidentialExpenseControl.Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<ResidentialExpenseControlContext>();
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<ITotalsRepository, TotalsRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<ITotalsService, TotalsService>();
            services.AddScoped<INotifier, Notifier>();

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            return services;
        }
    }
}
