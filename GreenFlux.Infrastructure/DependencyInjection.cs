using GreenFlux.Domain.Interfaces;
using GreenFlux.Infrastructure.Data;
using GreenFlux.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GreenFlux.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDI(this IServiceCollection services, IConfiguration configuration)
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

            services.AddMemoryCache();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IConnectorRepository, ConnectorRepository>();
            services.AddScoped<IChargeStationRepository, ChargeStationRepository>();

            return services;
        }
    }
}