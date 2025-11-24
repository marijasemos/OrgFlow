using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OegFlow.Domain.DTOs;
using OrgFlow.Domain.Entites;

namespace OrgFlow.Infrastructure.Interfaces
{
    public interface IOrganizationRepository
    {
        Task<Organization> CreateAsync(Organization organization);
        Task<Organization?> GetByIdAsync(int id);
        Task<IReadOnlyList<Organization>> GetAllAsync();

        Task<OrganizationPaginated> GetPaginatedAsync(int page, int pageSize);
        Task<Organization> UpdateAsync(Organization organization);
        Task DeleteAsync(int id);
    }
}
