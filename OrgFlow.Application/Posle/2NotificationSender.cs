using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace OrgFlow.Application.Posle
{
    public interface INotificationSender
    {
        string Channel { get; } // "email", "slack", "sms"...
        Task SendAsync(string to, string message);
    }

    public class EmailNotificationSender : INotificationSender
    {
        private readonly string _smtpServer = "smtp.orgflow.local";

        public string Channel => "email";

        public async Task SendAsync(string to, string message)
        {
            using var smtp = new SmtpClient(_smtpServer);
            var mail = new MailMessage("noreply@orgflow.com", to)
            {
                Subject = "OrgFlow notification",
                Body = message
            };

            await smtp.SendMailAsync(mail);
        }
    }

    public class SlackNotificationSender : INotificationSender
    {
        public string Channel => "slack";

        public Task SendAsync(string to, string message)
        {
            Console.WriteLine($"[SLACK → {to}] {message}");
            return Task.CompletedTask;
        }
    }

    public class SmsNotificationSender : INotificationSender
    {
        public string Channel => "sms";

        public Task SendAsync(string to, string message)
        {
            Console.WriteLine($"[SMS → {to}] {message}");
            return Task.CompletedTask;
        }
    }
}
