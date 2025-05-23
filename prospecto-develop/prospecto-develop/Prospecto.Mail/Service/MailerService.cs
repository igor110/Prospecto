using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Prospecto.Mail.Exceptions;
using Prospecto.Mail.Interfaces;
using Prospecto.Mail.Models;
using Prospecto.Mail.Models.Config;
using Prospecto.Options.Models;
using System;
using System.Net;

namespace Prospecto.Mail.Service
{
    //https://stackoverflow.com/questions/18503333/the-smtp-server-requires-a-secure-connection-or-the-client-was-not-authenticated

    public class MailerService : IMailerService
    {
        private readonly ILogger<MailerService> _logger;
        private readonly MailOptions _mailOptions;

        public MailerService(
            IOptions<MailOptions> mailOptions,
            ILogger<MailerService> logger)
        {
            _logger = logger;
            _mailOptions = mailOptions.Value;
        }

        public void SendMail(MailMessage message, BaseConfig config = null)
        {
            if (message == null)
                throw MessageMissingException.Create();

            try
            {
                using var createMessage = CreateMessage(message);
                System.Net.Mail.SmtpClient client = new(_mailOptions.Host, _mailOptions.Port);
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                NetworkCredential cred = new(_mailOptions.Username, _mailOptions.Password);
                client.Credentials = cred;
                client.Send(createMessage);
            }
            catch (SmtpException ex)
            {
                _logger.LogError(ex, "Email não enviado ", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                SmtpException.Create(ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Email não enviado ", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                throw MailGenericException.Create(ex);
            }
        }

        protected virtual System.Net.Mail.MailMessage CreateMessage(MailMessage message)
        {
            if (message == null)
                throw MessageMissingException.Create();

            System.Net.Mail.MailMessage request = new()
            {
                From = new System.Net.Mail.MailAddress(_mailOptions.Username, _mailOptions.Name),
                HeadersEncoding = System.Text.Encoding.UTF8,
                Subject = message.Subject,
                SubjectEncoding = System.Text.Encoding.UTF8,
                Body = !string.IsNullOrEmpty(message.Body.Html) ? message.Body.Html : message.Body.Text,
                BodyEncoding = System.Text.Encoding.UTF8,
                IsBodyHtml = !string.IsNullOrEmpty(message.Body.Html)
            };

            foreach (var item in message.To)
            {
                request.To.Add(item);
            }

            if (message.Bcc != null)
            {
                foreach (var item in message.Bcc)
                {
                    request.Bcc.Add(item);
                }
            }

            if (message.Cc != null)
            {
                foreach (var item in message.Cc)
                {
                    request.CC.Add(item);
                }
            }


            return request;
        }
    }
}
