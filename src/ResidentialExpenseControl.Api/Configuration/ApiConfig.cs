using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using ResidentialExpenseControl.Api.Filters;
using ResidentialExpenseControl.Infrastructure.Context;
using ResidentialExpenseControl.Infrastructure.Seed;
using System.Globalization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ResidentialExpenseControl.Api.Configuration
{
    public static class ApiConfig
    {
        public static IServiceCollection AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin();
                        builder.AllowAnyHeader();
                        builder.AllowAnyMethod();
                    });
            });

            services.AddControllers(options => options.Filters.Add(typeof(CustomModelStateValidationFilterAttribute)))
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Include;
                });

            services.AddControllersWithViews()
                .AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            services.AddDbContext<ResidentialExpenseControlContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.Configure<ApiBehaviorOptions>(options =>
                options.SuppressModelStateInvalidFilter = true
            );

            return services;
        }

        public static async Task<IApplicationBuilder> UseDatabaseMigration(this IApplicationBuilder app, WebApplication WebApplication)
        {
            using (var scope = WebApplication.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ResidentialExpenseControlContext>();
                await db.Database.MigrateAsync();
                await DatabaseSeeder.SeedAsync(db);
            }

            return app;
        }

        public static IApplicationBuilder UseApiConfiguration(this IApplicationBuilder app, 
            IWebHostEnvironment env, IConfiguration configuration)
        {
            app.UseDefaultFiles();

            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var cultureInfo = new CultureInfo("pt-BR");

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            app.UsePathBase("/api");

            app.UseRouting();

            app.UseCors("AllowAllOrigins");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            return app;
        }
    }
}
