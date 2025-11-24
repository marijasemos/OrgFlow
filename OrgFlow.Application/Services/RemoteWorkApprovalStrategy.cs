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
    public class RemoteWorkApprovalStrategy : IApprovalStrategy
    {
        public bool CanHandle(RequestBase request)
            => request is RemoteWorkRequest;

        public Task ApplyAsync(RequestBase request)
        {
            if (request is not RemoteWorkRequest rw)
                throw new ArgumentException("Invalid request type passed to RemoteWorkApprovalStrategy");

            rw.Status = RequestStatus.InReview;
            rw.UpdatedAt = DateTime.UtcNow;

            return Task.CompletedTask;
        }

    }
}
