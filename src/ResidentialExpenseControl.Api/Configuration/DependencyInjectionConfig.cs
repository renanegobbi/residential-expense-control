using Microsoft.Extensions.DependencyInjection;
using ResidentialExpenseControl.Core.Infrastructure;
using ResidentialExpenseControl.Domain.Interfaces;
using ResidentialExpenseControl.Domain.Interfaces.Infrastructure.Repositories;
using ResidentialExpenseControl.Domain.Interfaces.Services;
using ResidentialExpenseControl.Domain.Notifications;
using ResidentialExpenseControl.Domain.Services;
using ResidentialExpenseControl.Infrastructure;
using ResidentialExpenseControl.Infrastructure.Context;
using ResidentialExpenseControl.Infrastructure.Repositories;

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
            
            services.AddScoped<IPersonService, PersonService>();

            services.AddScoped<INotifier, Notifier>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
