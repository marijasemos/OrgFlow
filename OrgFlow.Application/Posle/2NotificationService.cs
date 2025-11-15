using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgFlow.Application.Posle
{
    /*Kako da im objasniš da je ovo OCP

Kažeš ovako:

“Pre OCP-a, kad god dodamo novi tip notifikacije, moramo da se vraćamo u kod i da dodajemo else if. To znači da je klasa otvorena za izmenu – što je loše.

Posle OCP-a, samo dodamo novu klasu koja implementira interfejs INotificationSender.
NotificationService ne diramo.

Dakle: proširujemo sistem dodavanjem novih klasa, a ne menjanjem postojećih. To je suština Open/Closed principa.” */
    public class _NotificationService
    {
        private readonly IMessageFormatter _formatter;

        private readonly NotificationSenderResolver _senderResolver;
        private readonly IAppLogger _logger;

        public _NotificationService(
            IMessageFormatter formatter,
            NotificationSenderResolver senderResolver,
            IAppLogger logger)
        {
            _formatter = formatter;
            _senderResolver = senderResolver;
            _logger = logger;
        }

        public async Task SendNotificationAsync(string channel, string to, string message)
        {
            // 1. formatiramo poruku
            var finalMessage = _formatter.Format(message);

            // 2. uzmemo odgovarajući sender BEZ if/else
            var sender = _senderResolver.GetSender(channel);

            // 3. pošaljemo
            await sender.SendAsync(to, finalMessage);

            // 4. ulogujemo
            _logger.Log($"Sent '{channel}' notification to {to}");
        }
    }
}
