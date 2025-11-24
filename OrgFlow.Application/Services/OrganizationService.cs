using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OegFlow.Domain.DTOs;
using OrgFlow.Api;
using OrgFlow.Application.Interfaces;
using OrgFlow.Domain.Entites;
using OrgFlow.Infrastructure;
using OrgFlow.Infrastructure.Interfaces;
using OrgFlow.Infrastructure.Services;

namespace OrgFlow.Application.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationRepository _organizationRepository;

        public OrganizationService(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
        }

        public async Task<Organization> CreateAsync(OrganizationDto organizationDto)
        {
           
          return await _organizationRepository.CreateAsync(new Organization() { Name = organizationDto.Name});
        }

        public async Task DeleteAsync(int id)
        {
            await _organizationRepository.DeleteAsync(id);
        }

        public async Task<IReadOnlyList<Organization>> GetAllAsync()
        {
            return await _organizationRepository.GetAllAsync();
        }

        public async Task<Organization?> GetByIdAsync(int id)
        {
            return await _organizationRepository.GetByIdAsync(id);
        }

        public async Task<OrganizationPaginated> GetPaginatedAsync(int page, int pageSize)
        {
            return await _organizationRepository.GetPaginatedAsync(page, pageSize);
        }

        public async Task<Organization> UpdateAsync(Organization organization)
        {
            return await _organizationRepository.UpdateAsync(organization);
        }
    }
}
