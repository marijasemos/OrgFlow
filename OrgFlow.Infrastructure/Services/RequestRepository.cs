using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrgFlow.Domain.Entities;
using OrgFlow.Infrastructure.Interfaces;

namespace OrgFlow.Infrastructure.Services
{
    public class RequestRepository : IRequestRepository
    {
        private readonly OrgFlowDbContext _context;

        public RequestRepository(OrgFlowDbContext context)
        {
            _context = context;
        }


        // -----------------------------
        // READ
        // -----------------------------

        public async Task<RequestBase?> GetByIdAsync(int id)
        {
            // Ef Core TPH: vratiće LeaveRequest ili RemoteWorkRequest instancu
            return await _context.Requests
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IReadOnlyList<RequestBase>> GetAllAsync()
        {
            return await _context.Requests
                .AsNoTracking()
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<LeaveRequest>> GetLeaveRequestsAsync()
        {
            // TPH: OfType<LeaveRequest>() filtrira po tipu
            return await _context.Requests
                .OfType<LeaveRequest>()
                .AsNoTracking()
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<RemoteWorkRequest>> GetRemoteWorkRequestsAsync()
        {
            return await _context.Requests
                .OfType<RemoteWorkRequest>()
                .AsNoTracking()
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        // -----------------------------
        // WRITE
        // -----------------------------

        public async Task AddAsync(RequestBase request)
        {
            await _context.Requests.AddAsync(request);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RequestBase request)
        {
            _context.Requests.Update(request);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _context.Requests.FirstOrDefaultAsync(r => r.Id == id);
            if (existing is null)
                return;

            _context.Requests.Remove(existing);
            await _context.SaveChangesAsync();
        }
    }
}
