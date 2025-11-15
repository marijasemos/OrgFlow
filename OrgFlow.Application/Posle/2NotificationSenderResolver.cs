using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgFlow.Application.Posle
{
    //3. Umesto onog starog RawNotificationSender sa if/else, sad imamo “resolver”

    //U DI svetu(ASP.NET Core) najlepše je da u servis ušpricaš listu svih sendera i da nađeš onaj koji treba.
    public class NotificationSenderResolver
    {
        private readonly IEnumerable<INotificationSender> _senders;

        public NotificationSenderResolver(IEnumerable<INotificationSender> senders)
        {
            _senders = senders;
        }

        public INotificationSender GetSender(string channel)
        {
            var sender = _senders.FirstOrDefault(s =>
                string.Equals(s.Channel, channel, StringComparison.OrdinalIgnoreCase));

            if (sender == null)
                throw new InvalidOperationException($"Notification channel '{channel}' not supported.");

            return sender;
        }
    }
}
