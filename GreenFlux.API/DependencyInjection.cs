using GreenFlux.Application;
using GreenFlux.Infrastructure;

namespace GreenFlux.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAppDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationDI();
            services.AddInfrastructureDI(configuration);
            return services;
        }
    }
}