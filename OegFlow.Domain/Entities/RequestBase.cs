using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrgFlow.Domain.Entites;
using OrgFlow.Domain.Enums;

namespace OrgFlow.Domain.Entities
{
    public class RequestBase
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public Organization? Organization { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int RequestTypeId { get; set; }

        public RequestType? RequestType { get; set; }
        public RequestStatus Status { get; set; } = RequestStatus.Draft;
        public int CreatedByUserId { get; set; }
        public User? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
