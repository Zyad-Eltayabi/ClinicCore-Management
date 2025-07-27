using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace ClinicAPI.Extensions;

public static class SwaggerExtensions
{
    public static Action<SwaggerGenOptions> Options()
    {
        return options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "ClinicCore Management",
                Description =
                    "ClinicCore Management is a comprehensive clinic administration system built with ASP.NET Core Web API.\n This system provides essential clinic management operations through a robust RESTful API designed specifically for healthcare administrators.\n",
                Contact = new OpenApiContact
                {
                    Name = "Zyad Eltayibi",
                    Email = "ZyadEltayibi@gmail.com",
                    Url = new Uri("https://github.com/Zyad-Eltayabi")
                },
                License = new OpenApiLicense
                {
                    Name = "MIT License",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                }
            });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));


            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Enter only your token.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        };
    }

    public static Action<SwaggerUIOptions> UiOptions()
    {
        return options =>
        {
            options.DocumentTitle = "ClinicCore Management API";
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "ClinicCore Management");
            options.RoutePrefix = "swagger";
            options.DisplayRequestDuration();
            options.EnableDeepLinking();
            options.EnableFilter();
            options.ShowExtensions();
            options.ShowCommonExtensions();
            options.SupportedSubmitMethods(
                SubmitMethod.Get,
                SubmitMethod.Post,
                SubmitMethod.Put,
                SubmitMethod.Delete
            );
        };
    }
}