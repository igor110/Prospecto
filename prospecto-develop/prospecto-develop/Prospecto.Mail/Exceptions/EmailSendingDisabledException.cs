using System;

namespace Prospecto.Mail.Exceptions
{
    public class SmtpException : BaseException
    {
        private SmtpException(string message, Exception innerException) : base(message, innerException) { }

        public static SmtpException Create(Exception innerException)
        {
            return new SmtpException("Smtp inválido ", innerException);
        }
    }
}
