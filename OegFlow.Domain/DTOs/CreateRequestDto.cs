using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgFlow.Domain.DTOs
{
    public class CreateRequestDto
    {
        // Shared fields ==============================

        public int OrganizationId { get; set; }        // Required
        public int RequestTypeId { get; set; }         // Required: Leave = 1, RemoteWork = 2...
        public int CreatedByUserId { get; set; }       // Required: Ko pravi zahtev

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Specific fields for LEAVE request ==========

        public DateTime? From { get; set; }            // LeaveRequest only
        public DateTime? To { get; set; }              // LeaveRequest only
        public string? Reason { get; set; }            // LeaveRequest only

        // Specific fields for REMOTE WORK request ====

        public DateTime? RemoteDate { get; set; }      // RemoteWorkRequest only
        public string? Location { get; set; }          // RemoteWorkRequest only

    }
}
