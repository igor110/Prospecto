using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Prospecto.Data.Extensions;
using Prospecto.Mail.Extensions;
using Prospecto.Models.Extensions;
using Prospecto.Respository.Extensions;
using Prospecto.Schedule;
using Prospecto.Schedule.Extensions;
using Prospecto.Service.Extensions;

namespace Prospecto.ViewMvc.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static void AddDependencyInjection(
          this IServiceCollection services,
          IWebHostEnvironment currentEnvironment,
          IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddHttpClient();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddModelsDependencyInjection();
            services.AddDataDependencyInjection();
            services.AddRepositoryDependencyInjection();
            services.AddServiceDependencyInjection(currentEnvironment);
            services.AddMailDependencyInjection();
            services.AddScheduleDependencyInjection();
            services.AddHostedService<QuartzHostedService>();            
            
        }
    }
}
