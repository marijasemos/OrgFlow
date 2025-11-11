using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrgFlow.Domain.Enums;

namespace OrgFlow.Domain.Entities
{
    public class UserRole
    {
        public int Id { get; set; }
        public RoleName Role { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
