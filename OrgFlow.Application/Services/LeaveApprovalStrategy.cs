using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrgFlow.Application.Configuration;
using OrgFlow.Application.Interfaces;
using OrgFlow.Domain.Entities;
using OrgFlow.Domain.Enums;

namespace OrgFlow.Application.Services
{
    public class LeaveApprovalStrategy : IApprovalStrategy
    {
        private readonly IApprovalPolicyProvaider _policy;

        public LeaveApprovalStrategy(IApprovalPolicyProvaider policy)
        {
            _policy = policy;
        }

        public bool CanHandle(RequestBase request)
        => request is LeaveRequest;
        public Task ApplyAsync(RequestBase request)
        {
            if (request is not LeaveRequest leave)
                throw new ArgumentException("Invalid request type passed to LeaveApprovalStrategy");

            var days = (leave.To - leave.From).TotalDays + 1;

            leave.Status = days <= _policy.MaxAutoApprovedLeaveDays
                ? RequestStatus.Approved : RequestStatus.InReview;

            leave.UpdatedAt = DateTime.UtcNow;
            return Task.CompletedTask;

        }

    }
}
