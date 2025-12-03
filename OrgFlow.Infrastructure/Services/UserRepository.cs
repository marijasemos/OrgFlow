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
    public class UserRepository
      : BaseRepository<User>, IUserRepository
    {
        public UserRepository(OrgFlowDbContext context)
            : base(context)
        {
        }

        public async Task<IReadOnlyList<User>> GetByDepartmentAsync(int departmetnId)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(u => u.DepartmentId == departmetnId && u.IsActive)
                .OrderBy(u => u.LastName).ThenBy(u => u.FirstName)
                .ToListAsync();
        }

     

        public async Task<IReadOnlyList<User>> GetByTeamAsync(int teamId)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(u => u.TeamId == teamId && u.IsActive)
                .OrderBy(u => u.LastName).ThenBy(u => u.FirstName)
                .ToListAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserBaseDataByEmailAsync(string email)
        {
            return await _dbSet
                .Include(u => u.Position)
                .Include(u => u.Department)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
