using Azure.Core;
using Microsoft.EntityFrameworkCore;
using OrgFlow.Domain.Entities;
using OrgFlow.Domain.Enums;
using OrgFlow.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgFlow.Infrastructure.Services
{
    public class RequestRepository :  BaseRepository<RequestBase>, IRequestRepository
    {
        private readonly OrgFlowDbContext _context;

        public RequestRepository(OrgFlowDbContext context) : base(context)
        {
            _context = context;
        }


  

        public async Task<IReadOnlyList<LeaveRequest>> GetLeaveRequestsAsync(CancellationToken token = default)
        {
            // TPH: OfType<LeaveRequest>() filtrira po tipu
            return await _context.Requests
                .OfType<LeaveRequest>()
                .AsNoTracking()
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync(token);
        }

        public async Task<IReadOnlyList<RemoteWorkRequest>> GetRemoteWorkRequestsAsync(CancellationToken token = default)
        {
            return await _context.Requests
                .OfType<RemoteWorkRequest>()
                .AsNoTracking()
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync(token);
        }

        public async Task<List<RequestBase>> GetStalePendingRequestsAsync(CancellationToken token = default)
        {
            return await _context.Requests
                .Where(x => x.Status == RequestStatus.InReview &&
                            x.CreatedAt < DateTime.UtcNow.AddHours(-2))
                .ToListAsync(token);
        }

    }
}
