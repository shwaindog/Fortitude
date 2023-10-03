#region

using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Trace;

#endregion

namespace FortitudeCommon.Monitoring;

public static class FrameworkInstrumentation
{
    public static void AddFrameworkOpenTelemetryInstrumentation(this IServiceCollection services)
    {
        services.AddOpenTelemetry()
            .WithTracing(builder => builder
                .AddConsoleExporter()
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation());
    }
}
