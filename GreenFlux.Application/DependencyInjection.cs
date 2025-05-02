using FluentValidation;
using GreenFlux.Application.Interfaces;
using GreenFlux.Application.Services;
using GreenFlux.Application.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace GreenFlux.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationDI(this IServiceCollection services)
        {
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IConnectorService, ConnectorService>();
            services.AddScoped<IChargeStationService, ChargeStationService>();

            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
            services.AddValidatorsFromAssemblyContaining<CreateGroupValidator>();
            return services;
        }
    }
}