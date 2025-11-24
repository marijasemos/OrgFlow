using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OegFlow.Domain.DTOs;
using OrgFlow.Api;
using OrgFlow.Domain.Entites;

namespace OrgFlow.Application.Interfaces
{
    public interface IOrganizationService
    {
        // CREATE
        Task<Organization> CreateAsync(OrganizationDto organizationDto);

        // READ
        Task<Organization?> GetByIdAsync(int id);
        Task<IReadOnlyList<Organization>> GetAllAsync();

        Task<OrganizationPaginated> GetPaginatedAsync(int page, int pageSize);

        // UPDATE
        Task<Organization> UpdateAsync(Organization organization);

        // DELETE
        Task DeleteAsync(int id);
    }
}
