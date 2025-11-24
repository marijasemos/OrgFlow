using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgFlow.Domain.Entities
{
    public class RemoteWorkRequest : RequestBase
    {
        public DateTime RemoteDate { get; set; }
        public string Location { get; set; } = string.Empty;
    }
}
