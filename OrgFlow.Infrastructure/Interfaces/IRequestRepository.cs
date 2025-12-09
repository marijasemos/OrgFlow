using Azure.Core;
using OegFlow.Domain.Entities;
using OrgFlow.Domain.DTOs;
using OrgFlow.Domain.Entites;
using OrgFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgFlow.Infrastructure.Interfaces
{
    public interface IRequestRepository : IBaseRepository<RequestBase>
    {
      
        Task<IReadOnlyList<LeaveRequest>> GetLeaveRequestsAsync(CancellationToken token = default);
        Task<IReadOnlyList<RemoteWorkRequest>> GetRemoteWorkRequestsAsync(CancellationToken token = default);
        Task<List<RequestBase>> GetStalePendingRequestsAsync(CancellationToken token = default);

    }
}
