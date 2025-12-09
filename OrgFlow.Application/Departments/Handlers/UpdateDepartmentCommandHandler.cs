using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using OegFlow.Domain.Entities;
using OrgFlow.Application.Departments.Commands;
using OrgFlow.Infrastructure.Interfaces;

namespace OrgFlow.Application.Departments.Handlers
{
    public class UpdateDepartmentCommandHandler
    : IRequestHandler<UpdateDepartmentCommand, Department>
    {
        private readonly IDepartmentRepository _repo;
        private readonly ILogger<UpdateDepartmentCommandHandler> _logger;

        public UpdateDepartmentCommandHandler(
            IDepartmentRepository repo,
            ILogger<UpdateDepartmentCommandHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<Department> Handle(
            UpdateDepartmentCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            var existing = await _repo.GetByIdAsync(dto.Id, cancellationToken)
                ?? throw new KeyNotFoundException($"Department {dto.Id} not found.");

            existing.OrganizationId = dto.OrganizationId;
            existing.Name = dto.Name;
            existing.Description = dto.Description;
            existing.IsActive = dto.IsActive;

            await _repo.UpdateAsync(existing, cancellationToken);
            _logger.LogInformation("Department {Id} updated", existing.Id);

            return existing;
        }
    }
}
