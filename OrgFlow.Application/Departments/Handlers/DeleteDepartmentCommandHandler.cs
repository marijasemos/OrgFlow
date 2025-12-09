using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using OrgFlow.Application.Departments.Commands;
using OrgFlow.Infrastructure.Interfaces;

namespace OrgFlow.Application.Departments.Handlers
{
    public class DeleteDepartmentCommandHandler
    : IRequestHandler<DeleteDepartmentCommand>
    {
        private readonly IDepartmentRepository _repo;
        private readonly ILogger<DeleteDepartmentCommandHandler> _logger;

        public DeleteDepartmentCommandHandler(
            IDepartmentRepository repo,
            ILogger<DeleteDepartmentCommandHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task Handle(
            DeleteDepartmentCommand request,
            CancellationToken cancellationToken)
        {
            await _repo.DeleteAsync(request.Id, cancellationToken);
            _logger.LogInformation("Department {Id} deleted (if existed)", request.Id);
        }
    }
}
