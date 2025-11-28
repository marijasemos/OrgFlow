using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace OrgFlow.Application.Configuration
{
    public class ApprovalPolicyProvider : IApprovalPolicyProvaider
    {
        private readonly ApprovalPolicySettings _settings;

        public ApprovalPolicyProvider(IOptions<ApprovalPolicySettings> options)
        {
            _settings = options.Value;
        }

        public int MaxAutoApprovedLeaveDays => _settings.MaxAutoApprovedLeaveDays;

        public bool IsRemoteWorkAutoApproved => _settings.IsRemoteWorkAutoApproved;
    }
}
