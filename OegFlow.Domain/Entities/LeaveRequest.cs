using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgFlow.Domain.Entities
{
    public class LeaveRequest : RequestBase
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string? Reason { get; set; }
    }
}
