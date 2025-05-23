using Microsoft.Extensions.Logging;
using Prospecto.Mail.Interfaces;
using Prospecto.Mail.Models;
using Prospecto.Models.Enums;
using Prospecto.Models.ViewModel;
using Prospecto.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prospecto.Mail.Service
{
    public class MessageService : IMessageService
    {
        private readonly ILogger<MessageService> _logger;
        private readonly IAttendanceService _attendanceService;
        private readonly IUserService _userService;

        public MessageService(
            ILogger<MessageService> logger,
            IUserService userService,
            IAttendanceService attendanceService)
        {
            _logger = logger;
            _attendanceService = attendanceService;
            _userService = userService;
        }

        public MailMessage CreateMessageScheduledService(int companyId)
        {
            _logger.LogInformation("Iniciando processo de enviar email!", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            
            IList<UserViewModel> lstUsers = _userService.ListByFilters(new UserFiltersViewModel { TypeUser = UserTypeEnum.MANAGER, CompanyId = companyId });
            IList<UserViewModel> lstUsersAdmin = _userService.ListByFilters(new UserFiltersViewModel { TypeUser = UserTypeEnum.ADMINISTRATOR });

            MailBody body = new();
            AttendanceFiltersViewModel filters = new()
            {
                BeginDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 00, 00, 00),
                EndDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59),
                CompanyId = companyId
            };

            IList<ScheduleServiceViewModel> attendanceViewModels = _attendanceService.ScheduledService(filters);
            body.Html = "<html><body>";
            for (int i1 = 0; i1 < attendanceViewModels.Count; i1++)
            {
                var attendanceViewModel = attendanceViewModels[i1];
                body.Html += attendanceViewModel.User + " - " + attendanceViewModel.AmountService + " <br>";
            }
            body.Html += "</body></html>";

            var message = new MailMessage()
            {
                Subject = "Atendimentos agendados do dia " + DateTime.Now,
                To = lstUsers.Select(user => user.Email).ToList(),
                Cc = lstUsersAdmin.Select(user => user.Email).ToList(),
                Body = body
            };

            return message;
        }
    }
}
