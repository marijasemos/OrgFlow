using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using OegFlow.Domain.Entities;
using OrgFlow.Application.Departments.Queries;
using OrgFlow.Infrastructure.Interfaces;

namespace OrgFlow.Application.Departments.Handlers
{
    public class GetDepartmentByIdQueryHandler
    : IRequestHandler<GetDepartmentByIdQuery, Department?>
    {
        private readonly IDepartmentRepository _repo;
        private readonly ILogger<GetDepartmentByIdQueryHandler> _logger;

        public GetDepartmentByIdQueryHandler(
            IDepartmentRepository repo,
            ILogger<GetDepartmentByIdQueryHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<Department?> Handle(
            GetDepartmentByIdQuery request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching department {Id}", request.Id);
            return await _repo.GetByIdAsync(request.Id, cancellationToken);
        }
    }
}
