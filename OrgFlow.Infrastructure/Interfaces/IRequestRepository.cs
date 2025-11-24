using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrgFlow.Domain.DTOs;
using OrgFlow.Domain.Entites;
using OrgFlow.Domain.Entities;

namespace OrgFlow.Infrastructure.Interfaces
{
    public interface IRequestRepository
    {
        Task<RequestBase?> GetByIdAsync(int id);
        Task<IReadOnlyList<RequestBase>> GetAllAsync();

        Task<IReadOnlyList<LeaveRequest>> GetLeaveRequestsAsync();
        Task<IReadOnlyList<RemoteWorkRequest>> GetRemoteWorkRequestsAsync();

        Task AddAsync(RequestBase request);
        Task UpdateAsync(RequestBase request);
        Task DeleteAsync(int id);
    }
}
