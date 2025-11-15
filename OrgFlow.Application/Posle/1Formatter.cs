using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgFlow.Application.Posle
{
    public interface IMessageFormatter
    {
        string Format(string rawMessage);
    }

    public class OrgFlowMessageFormatter : IMessageFormatter
    {
        public string Format(string rawMessage)
        {
            return $"[OrgFlow Notification] {rawMessage}";
        }
    }
}
