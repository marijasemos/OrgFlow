using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgFlow.Application.Configuration
{
    public interface IApprovalPolicyProvaider
    {
        int MaxAutoApprovedLeaveDays { get; }
        bool IsRemoteWorkAutoApproved { get; }
    }
}
