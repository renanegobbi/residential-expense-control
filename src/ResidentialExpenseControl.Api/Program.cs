
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ResidentialExpenseControl.Api.Configuration;
using System.Threading.Tasks;

namespace ResidentialExpenseControl.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddApiConfiguration(builder.Configuration);

            builder.Services.ResolveDependencies();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen();

            builder.Services.AddSwaggeConfiguration();


            var app = builder.Build();

            await app.UseDatabaseMigration(app);

            app.UseApiConfiguration(app.Environment, app.Configuration);

            app.UseSwaggerConfiguration(app.Environment);

            app.Run();
        }

    }
}