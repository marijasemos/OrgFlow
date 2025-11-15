using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgFlow.Application.Posle
{
    public class NotificationService
    {
        private readonly IMessageFormatter _formatter;
        private readonly IRawNotificationSender _sender;
        private readonly IAppLogger _logger;

        public NotificationService(
            IMessageFormatter formatter,
            IRawNotificationSender sender,
            IAppLogger logger)
        {
            _formatter = formatter;
            _sender = sender;
            _logger = logger;
        }

        public async Task SendNotificationAsync(string notificationType, string to, string message)
        {
            // 1. formatiranje
            var finalMessage = _formatter.Format(message);

            // 2. slanje (i dalje ružno bira kanal, ali to je sad odgovornost sendera)
            await _sender.SendRawAsync(notificationType, to, finalMessage);

            // 3. logovanje
            _logger.Log($"Sent '{notificationType}' notification to {to}");
        }
    }
}
