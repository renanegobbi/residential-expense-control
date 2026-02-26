using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Filters;

namespace ResidentialExpenseControl.Api.Configuration
{
    public static class SwaggerConfig
    {
        public static IServiceCollection AddSwaggeConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen();

            services.AddSwaggerExamplesFromAssemblyOf<Program>();

            return services;
        }

        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app, 
            IWebHostEnvironment webHostEnvironment)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            return app;
        }
    }
         
}


