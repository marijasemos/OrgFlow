using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace OrgFlow.Application.PRE
{
    public class NotificationService
    {
        private readonly string _smtpServer = "smtp.orgflow.local";

        public async Task SendNotificationAsync(
            string notificationType,   // "email", "slack", "sms"...
            string to,
            string message)
        {
            // 1. formatiranje poruke (posao #1)
            var finalMessage = $"[OrgFlow Notification] {message}";

            // 2. izbor kanala preko if-else (posao #2, krši OCP)
            if (notificationType == "email")
            {
                // 3. direktno slanje mejla (posao #3, krši DIP)
                using var smtp = new SmtpClient(_smtpServer);
                var mail = new MailMessage("noreply@orgflow.com", to);
                mail.Subject = "OrgFlow notification";
                mail.Body = finalMessage;
                await smtp.SendMailAsync(mail);
            }
            else if (notificationType == "slack")
            {
                // umesto interfejsa mi ovde “glumimo” slanje na Slack
                Console.WriteLine($"[SLACK → {to}] {finalMessage}");
            }
            else if (notificationType == "sms")
            {
                // glumimo SMS
                Console.WriteLine($"[SMS → {to}] {finalMessage}");
            }
            else
            {
                throw new InvalidOperationException("Notification type not supported.");
            }

            // 4. logovanje direktno u fajl (posao #4)
            File.AppendAllText("notifications.log",
                $"{DateTime.UtcNow}: Sent '{notificationType}' notification to {to}{Environment.NewLine}");
        }
    }
}