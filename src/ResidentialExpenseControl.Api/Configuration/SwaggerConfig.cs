using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Filters;
using System.IO;
using System.Reflection;
using System;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace ResidentialExpenseControl.Api.Configuration
{
    public static class SwaggerConfig
    {
        public static IServiceCollection AddSwaggeConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<SwaggerDefaultValues>();

                options.EnableAnnotations();

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                options.ExampleFilters();
                options.IncludeXmlComments(xmlPath);
                options.EnableAnnotations();
            });

            services.AddSwaggerExamplesFromAssemblyOf<Program>();

            return services;
        }

        public class GetStreamManifestResource
        {
            public Stream ManifestResourceStream() => GetType().GetTypeInfo().GetTypeInfo()
                .Assembly.GetManifestResourceStream("ResidentialExpenseControl.Api.Swagger.UI.Index.html");
        }

        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app,
            IApiVersionDescriptionProvider provider, IWebHostEnvironment webHostEnvironment)
        {
            app.UseSwagger(options =>
            {
                options.RouteTemplate = "docs/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = "docs";
                options.DocumentTitle = "API ResidentialExpenseControl - Documentation";

                options.DocExpansion(DocExpansion.None);

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/api/docs/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }

                options.InjectStylesheet("/swagger-ui/custom.css");
                options.InjectStylesheet($"/swagger-ui/custom-{webHostEnvironment.EnvironmentName}.css");

                options.IndexStream = () => new GetStreamManifestResource().ManifestResourceStream();

                options.EnableValidator(null);
            });

            return app;
        }
    }

    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider provider;

        readonly IWebHostEnvironment webHostEnvironment;

        readonly IConfiguration configuration;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            this.provider = provider;
            this.webHostEnvironment = webHostEnvironment;
            this.configuration = configuration;
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description, webHostEnvironment, this.configuration));
            }
        }

        static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description,
           IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            var environmentName = webHostEnvironment.EnvironmentName;

            var customDescription = "API for managing residential expenses, including people, categories, transactions, and financial summaries..<br>" +
                                "<ul>" +
                                    $"<li>Current environment: <b>{environmentName.ToUpper()}</b>.</li>" +
                                    $"<li>Database (SQLite): <b>residential-expense.db</b>.</li>" +
                                "</ul>";

            customDescription += "<p>Contact information:</p>" +
                           "<ul>" +
                                "<li>Email: <b>renan@email.com</b></li>" +
                           "</ul>";

            var info = new OpenApiInfo()
            {
                Title = "ResidentialExpenseControl",
                Version = description.ApiVersion.ToString(),
                Description = customDescription,
            };

            if (description.IsDeprecated)
            {
                info.Description += " This version is obsolete!";
            }

            return info;
        }
    }

    public class SwaggerDefaultValues : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                return;
            }

            foreach (var parameter in operation.Parameters)
            {
                var description = context.ApiDescription
                    .ParameterDescriptions
                    .First(p => p.Name == parameter.Name);

                var routeInfo = description.RouteInfo;

                operation.Deprecated = OpenApiOperation.DeprecatedDefault;

                if (parameter.Description == null)
                {
                    parameter.Description = description.ModelMetadata?.Description;
                }

                if (routeInfo == null)
                {
                    continue;
                }

                if (parameter.In != ParameterLocation.Path && parameter.Schema.Default == null)
                {
                    parameter.Schema.Default = new OpenApiString(routeInfo.DefaultValue.ToString());
                }

                parameter.Required |= !routeInfo.IsOptional;
            }
        }
    }

}


