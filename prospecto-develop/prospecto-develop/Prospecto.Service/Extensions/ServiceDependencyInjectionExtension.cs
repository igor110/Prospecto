using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prospecto.Service.Interface;

namespace Prospecto.Service.Extensions
{
    public static class ServiceDependencyInjectionExtension
    {
        public static void AddServiceDependencyInjection(
         this IServiceCollection services,
         IHostEnvironment currentEnvironment)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IAttendanceService, AttendanceService>();
            services.AddScoped<IBranchService, BranchService>();
            services.AddScoped<IClientService, ClientService>();
        }
    }
}
