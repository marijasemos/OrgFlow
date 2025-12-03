using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrgFlow.Domain.Entities;

namespace OrgFlow.Infrastructure.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<IReadOnlyList<User>> GetByDepartmentAsync(int departmentId);
        Task<IReadOnlyList<User>> GetByTeamAsync(int teamId);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetUserBaseDataByEmailAsync(string email);
    }
}
