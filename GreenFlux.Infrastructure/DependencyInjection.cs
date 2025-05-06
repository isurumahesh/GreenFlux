using GreenFlux.Domain.Interfaces;
using GreenFlux.Infrastructure.Data;
using GreenFlux.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Json;

namespace GreenFlux.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDI(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<GreenFluxDbContext>((provider, options) =>
            {
                options.UseSqlServer(connectionString, sqlServerOptions =>
                {
                    sqlServerOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(3),
                        errorNumbersToAdd: null
                    );
                });
            });

            Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File(new JsonFormatter(), "logs/log-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog();
            });

            if (environment.IsDevelopment())
            {
                using var serviceProvider = services.BuildServiceProvider();
                try
                {
                    var context = serviceProvider.GetRequiredService<GreenFluxDbContext>();
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("MigrationLogger");
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }

            services.AddMemoryCache();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IConnectorRepository, ConnectorRepository>();
            services.AddScoped<IChargeStationRepository, ChargeStationRepository>();

            return services;
        }
    }
}