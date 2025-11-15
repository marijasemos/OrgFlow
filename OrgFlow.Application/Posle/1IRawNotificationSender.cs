using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace OrgFlow.Application.Posle
{
    public interface IRawNotificationSender
    {
        Task SendRawAsync(string notificationType, string to, string formattedMessage);
    }

    public class RawNotificationSender : IRawNotificationSender
    {
        private readonly string _smtpServer = "smtp.orgflow.local";

        public async Task SendRawAsync(string notificationType, string to, string formattedMessage)
        {
            if (notificationType == "email")
            {
                using var smtp = new SmtpClient(_smtpServer);
                var mail = new MailMessage("noreply@orgflow.com", to)
                {
                    Subject = "OrgFlow notification",
                    Body = formattedMessage
                };
                await smtp.SendMailAsync(mail);
            }
            else if (notificationType == "slack")
            {
                Console.WriteLine($"[SLACK → {to}] {formattedMessage}");
            }
            else if (notificationType == "sms")
            {
                Console.WriteLine($"[SMS → {to}] {formattedMessage}");
            }
            else
            {
                throw new InvalidOperationException("Notification type not supported.");
            }
        }
    }
}
