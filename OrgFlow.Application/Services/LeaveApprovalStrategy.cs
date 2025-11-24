using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrgFlow.Application.Interfaces;
using OrgFlow.Domain.Entities;
using OrgFlow.Domain.Enums;

namespace OrgFlow.Application.Services
{
    public class LeaveApprovalStrategy : IApprovalStrategy
    {

        public bool CanHandle(RequestBase request)
        => request is LeaveRequest;
        public Task ApplyAsync(RequestBase request)
        {
            if (request is not LeaveRequest leave)
                throw new ArgumentException("Invalid request type passed to LeaveApprovalStrategy");

            var days = (leave.To - leave.From).TotalDays + 1;

            leave.Status = days <= 3
                ? RequestStatus.Approved : RequestStatus.InReview;

            leave.UpdatedAt = DateTime.UtcNow;
            return Task.CompletedTask;

        }

    }
}
