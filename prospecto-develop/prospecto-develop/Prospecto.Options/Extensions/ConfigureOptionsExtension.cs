using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prospecto.Options.Models;

namespace Prospecto.Options.Extensions
{
    public static class ConfigureOptionsExtension
    {
        public static void ConfigureProspectoOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailOptions>(configuration.GetSection(MailOptions.SECTION_NAME));
        }
    }
}
