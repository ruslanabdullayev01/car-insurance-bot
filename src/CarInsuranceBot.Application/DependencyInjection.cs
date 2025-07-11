using Microsoft.Extensions.DependencyInjection;

namespace CarInsuranceBot.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMediatr(this IServiceCollection services)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
              
            });
            return services;
        }
    }
}
