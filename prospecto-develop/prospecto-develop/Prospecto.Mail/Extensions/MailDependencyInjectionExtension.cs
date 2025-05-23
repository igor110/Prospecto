using Microsoft.Extensions.DependencyInjection;
using Prospecto.Mail.Interfaces;
using Prospecto.Mail.Service;

namespace Prospecto.Mail.Extensions
{
    public static class MailDependencyInjectionExtension
    {
        public static void AddMailDependencyInjection(
        this IServiceCollection services)
        {
            services.AddScoped<IMailerService, MailerService>();
            services.AddScoped<IMessageService, MessageService>();
        }
    }
}
