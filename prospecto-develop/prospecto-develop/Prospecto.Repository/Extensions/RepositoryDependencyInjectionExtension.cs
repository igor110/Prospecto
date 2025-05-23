using Microsoft.Extensions.DependencyInjection;
using Prospecto.Repository;
using Prospecto.Respository.Interface;

namespace Prospecto.Respository.Extensions
{
    public static class RepositoryDependencyInjectionExtension
    {
        public static void AddRepositoryDependencyInjection(
         this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IAttendanceRepository, AttendanceRepository>();
            services.AddScoped<IBranchRepository, BranchRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
        }
    }
}
