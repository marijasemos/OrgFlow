using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgFlow.Application.Posle
{
    public interface IAppLogger
    {
        void Log(string message);
    }

    public class FileAppLogger : IAppLogger
    {
        public void Log(string message)
        {
            File.AppendAllText("notifications.log",
                $"{DateTime.UtcNow}: {message}{Environment.NewLine}");
        }
    }
}
