using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OegFlow.Domain.DTOs;
using OrgFlow.Domain.Entites;
using OrgFlow.Infrastructure.Interfaces;

namespace OrgFlow.Infrastructure.Services
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly OrgFlowDbContext _context;
        public OrganizationRepository()
        {
            var options = new DbContextOptionsBuilder<OrgFlowDbContext>()
                .UseSqlServer("Server=MARIJA;Database=OrgFlow; Trusted_Connection=true; MultipleActiveResultSets = true; TrustServerCertificate=true")
                .Options;

            _context = new OrgFlowDbContext(options);

        }


        public async Task<Organization> CreateAsync(Organization organization)
        {
            // Po želji možeš da dodaš validacije (npr. da Name nije prazan)
            await _context.Organizations.AddAsync(organization);
            await _context.SaveChangesAsync();
            return organization;
        }

        public async Task<Organization?> GetByIdAsync(int id)
        {
            // Ako želiš da odmah učitaš i navigacione kolekcije:
            return await _context.Organizations
                .Include(o => o.Users)
                .Include(o => o.Requests)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IReadOnlyList<Organization>> GetAllAsync()
        {
            // Ako ne moraš da učitavaš Users/Requests, izbaci Include da bude brže
            return await _context.Organizations
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Organization> UpdateAsync(Organization organization)
        {
            // Pretpostavljamo da entity već postoji; možeš prethodno da ga proveriš
            _context.Organizations.Update(organization);
            await _context.SaveChangesAsync();
            return organization;
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _context.Organizations.FindAsync(id);
            if (existing == null)
            {
                // Po želji baci custom exception ili samo quietly return
                return;
            }

            _context.Organizations.Remove(existing);
            await _context.SaveChangesAsync();
        }

        public async Task<OrganizationPaginated> GetPaginatedAsync(int page, int pageSize)
        {
            var query = _context.Organizations.AsQueryable();

            // 1. Ukupan broj zapisa
            var totalCount = await query.CountAsync();

            // 2. Paginacija
            var items = await query
                .OrderBy(o => o.Id)                 // uvek definiši order!!!
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return new OrganizationPaginated
            {
                Organizations = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}