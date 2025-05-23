using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Prospecto.Mail.Interfaces;
using Prospecto.Models.ViewModel;
using Prospecto.Service.Interface;
using Quartz;
using System;
using System.Threading.Tasks;

namespace Prospecto.Schedule
{
    [DisallowConcurrentExecution]
    public class ScheduledAppointments : IJob
    {
        private readonly ILogger<ScheduledAppointments> _logger;
        private readonly IServiceProvider _serviceProvider;

        public ScheduledAppointments(
            IServiceProvider serviceProvider,
            ILogger<ScheduledAppointments> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        Task IJob.Execute(IJobExecutionContext context)
        {
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {                
                var _mailerService = scope.ServiceProvider.GetService<IMailerService>();
                var _messageService = scope.ServiceProvider.GetService<IMessageService>();
                var _companyService = scope.ServiceProvider.GetService<ICompanyService>();

                foreach (var company in _companyService.ListByFilters(new CompanyFiltersViewModel()))
                {
                    _mailerService.SendMail(_messageService.CreateMessageScheduledService(company.Id));                    
                    _logger.LogInformation("Realizado o envio do agendamentos por email para empresa " + company.Description, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                } 
            }
            return Task.CompletedTask;
        }        
    }
}
