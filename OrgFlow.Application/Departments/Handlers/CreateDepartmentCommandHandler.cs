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
    public class CreateDepartmentCommandHandler
    : IRequestHandler<CreateDepartmentCommand, Department>
    {
        private readonly IDepartmentRepository _repo;
        private readonly ILogger<CreateDepartmentCommandHandler> _logger;

        public CreateDepartmentCommandHandler(
            IDepartmentRepository repo,
            ILogger<CreateDepartmentCommandHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<Department> Handle(
            CreateDepartmentCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Department name is required.");

            var department = new Department
            {
                OrganizationId = dto.OrganizationId,
                Name = dto.Name,
                Description = dto.Description,
                IsActive = true
            };

            await _repo.AddAsync(department, cancellationToken);
            _logger.LogInformation("Department {Id} created", department.Id);

            return department;
        }
    }
}
