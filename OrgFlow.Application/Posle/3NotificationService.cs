using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgFlow.Application.Posle
{
    /* Umesto da zavisi od “resolver” klase, zavisi direktno od enumeracije apstrakcija (IEnumerable<INotificationSender>).
To je čist DIP: visoki nivo (servis) zna samo za interfejse.*/
    public class NotificationService1
    {
        private readonly IMessageFormatter _formatter;
       // private readonly NotificationSenderResolver _senderResolver;
        private readonly IEnumerable<INotificationSender> _senders;
        private readonly IAppLogger _logger;

        public NotificationService1(
            IMessageFormatter formatter,
            IEnumerable<INotificationSender> senders,
            IAppLogger logger)
        {
            _formatter = formatter;
            _senders = senders;
            _logger = logger;
        }

        public async Task SendNotificationAsync(string channel, string to, string message)
        {
            // 1) apstraktno formatiranje (ne znamo kako radi, niti nas zanima)
            var finalMessage = _formatter.Format(message);

            // 2) biramo implementaciju po kanalu (i dalje bez if/else kaskade po tipu)
            var sender = _senders.FirstOrDefault(s =>
                string.Equals(s.Channel, channel, StringComparison.OrdinalIgnoreCase));

            if (sender == null)
                throw new InvalidOperationException($"Notification channel '{channel}' not supported.");

            // 3) šaljemo apstraktno
            await sender.SendAsync(to, finalMessage);

            // 4) logujemo apstraktno
            _logger.Log($"Sent '{channel}' notification to {to}");
        }
    }
}
