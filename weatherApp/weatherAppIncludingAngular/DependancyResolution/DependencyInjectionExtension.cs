﻿//Change History
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// 12/01/2022 Ticket1 JS Team darkSaber - Initial version. 
// --------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace weatherApp.DependancyResolution
{
    using Microsoft.Extensions.DependencyInjection;
    using weatherApp.Mappers;
    using weatherApp.Service;
    using weatherApp.BusinessLogic;

    public static class DependencyInjectionExtension
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IWeatherService, StandardWeatherService>();
            services.AddScoped<IWeatherOrchestrator, StandardWeatherOrchestrator>();
            services.AddScoped<IForecastSummaryMapper, StandardSummaryMapper>();
            services.AddScoped<IForecastMapper, StandardForecastMapper>();
            services.AddScoped<IAstronomyMapper, StandardAstronomyMapper>();
            services.AddScoped<IAstronomySummaryMapper, StandardAstronomySummaryMapper>();
            services.AddScoped<IErrorMapper, StandardErrorMapper>();
            return services;
        }
    }
}
