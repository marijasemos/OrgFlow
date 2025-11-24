using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrgFlow.Domain.DTOs;
using OrgFlow.Domain.Entities;

namespace OrgFlow.Application.Helpers
{
    public class RequestValidator
    {
        public List<string> ValidateDto(CreateRequestDto dto)
        {
            var errors = new List<string>();

            if (dto.OrganizationId <= 0)
                errors.Add("OrganizationId must be > 0.");

            if (string.IsNullOrWhiteSpace(dto.Title))
                errors.Add("Title is required.");

            if (dto.RequestTypeId == 1 && (!dto.From.HasValue || !dto.To.HasValue))
                errors.Add("Leave requests require From and To dates.");

            if (dto.RequestTypeId == 2 && !dto.RemoteDate.HasValue)
                errors.Add("Remote requests require RemoteDate.");

            return errors;
        }

        public List<string> ValidateEntity(RequestBase request)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(request.Title))
                errors.Add("Title is required.");

            if (request.OrganizationId <= 0)
                errors.Add("OrganizationId must be > 0");

            return errors;
        }
    }
}
