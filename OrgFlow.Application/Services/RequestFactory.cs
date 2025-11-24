using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrgFlow.Application.Interfaces;
using OrgFlow.Domain.DTOs;
using OrgFlow.Domain.Entities;
using OrgFlow.Domain.Enums;

namespace OrgFlow.Application.Services
{
    public class RequestFactory : IRequestFactory
    {
        public RequestBase Create(CreateRequestDto dto)
        {
            var requestType = (RequestType)dto.RequestTypeId;

            return requestType switch
            {
                RequestType.Leave => CreateLeaveRequest(dto),
                RequestType.RemoteWork => CreateRemoteWorkRequest(dto),

                _ => throw new ArgumentOutOfRangeException(nameof(dto.RequestTypeId),
                       $"Unknown request type: {dto.RequestTypeId}")
            };
        }

        private LeaveRequest CreateLeaveRequest(CreateRequestDto dto)
        {
            if (!dto.From.HasValue || !dto.To.HasValue)
                throw new ArgumentException("Leave request requires From and To dates.");

            var request = new LeaveRequest
            {
                OrganizationId = dto.OrganizationId,
                Title = dto.Title,
                Description = dto.Description,
                RequestType = (RequestType)dto.RequestTypeId,
                CreatedByUserId = dto.CreatedByUserId,
                From = dto.From.Value,
                To = dto.To.Value,
                Reason = dto.Reason,
                Status = RequestStatus.Draft,
                CreatedAt = DateTime.UtcNow
            };

            return request;
        }

        private RemoteWorkRequest CreateRemoteWorkRequest(CreateRequestDto dto)
        {
            if (!dto.RemoteDate.HasValue)
                throw new ArgumentException("Remote work request requires RemoteDate.");

            var request = new RemoteWorkRequest
            {
                OrganizationId = dto.OrganizationId,
                Title = dto.Title,
                Description = dto.Description,
                RequestType = (RequestType)dto.RequestTypeId,
                CreatedByUserId = dto.CreatedByUserId,
                RemoteDate = dto.RemoteDate.Value,
                Location = dto.Location ?? "Home",
                Status = RequestStatus.Draft,
                CreatedAt = DateTime.UtcNow
            };

            return request;
        }

    }
}
