using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Prospecto.Data.Extensions
{
    public static class DataDependencyInjectionExtension
    {
        public static void AddDataDependencyInjection(
         this IServiceCollection services)
        {
            services.AddDbContext<DbContext, ProspectoContext>();
        }
    }
}
