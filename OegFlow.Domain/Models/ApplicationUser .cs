using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using OegFlow.Domain.Entities;
using OrgFlow.Domain.Entites;
using OrgFlow.Domain.Entities;

namespace OegFlow.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public int  DepartmentId { get; set; }
        public Department  Department { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
