using Serilog;

namespace ClinicAPI.Extensions;

public static class SerilogExtensions
{
    public static WebApplicationBuilder UseSerilogRequestLogging(this WebApplicationBuilder builder)
    {
        // Configure the HostBuilder to use Serilog as the logging provider.
        builder.Host.UseSerilog((context, services, configuration) =>
        {
            // Reads Serilog settings from appsettings.json (or other configuration sources)
            configuration.ReadFrom.Configuration(context.Configuration);
        });
        return builder;
    }
}