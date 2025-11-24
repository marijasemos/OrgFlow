using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrgFlow.Domain.Entites;

namespace OegFlow.Domain.DTOs
{
    public class OrganizationPaginated
    {
        public IReadOnlyList<Organization> Organizations { get; set; }
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
