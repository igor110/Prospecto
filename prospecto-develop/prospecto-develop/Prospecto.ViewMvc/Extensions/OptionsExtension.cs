using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prospecto.Options.Extensions;

namespace Prospecto.ViewMvc.Extensions
{
    public static class OptionsExtension
    {
        public static void ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureProspectoOptions(configuration);
        }
    }
}
