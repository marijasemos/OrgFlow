using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgFlow.Application.Configuration
{
    public class ApprovalPolicySettings
    {
        public int MaxAutoApprovedLeaveDays { get; set; }
        public bool IsRemoteWorkAutoApproved { get; set; }
    }
}
