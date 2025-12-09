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
    public class GetAllDepartmentsQueryHandler
     : IRequestHandler<GetAllDepartmentsQuery, IReadOnlyList<Department>>
    {
        private readonly IDepartmentRepository _repo;
        private readonly ILogger<GetAllDepartmentsQueryHandler> _logger;

        public GetAllDepartmentsQueryHandler(
            IDepartmentRepository repo,
            ILogger<GetAllDepartmentsQueryHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<IReadOnlyList<Department>> Handle(
            GetAllDepartmentsQuery request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching all departments");
            return await _repo.GetAllAsync(cancellationToken);
        }
    }
}
