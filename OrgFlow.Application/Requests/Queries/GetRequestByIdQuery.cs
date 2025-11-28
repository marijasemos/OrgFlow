using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using OrgFlow.Domain.Entities;
using OrgFlow.Infrastructure.Interfaces;

namespace OrgFlow.Application.Requests.Queries
{
    public record GetRequestByIdQuery(int Id) : IRequest<RequestBase?>;

    public class GetRequestByIdQueryHandler
    : IRequestHandler<GetRequestByIdQuery, RequestBase?>
    {
        private readonly IRequestRepository _repo;
        private readonly ILogger<GetRequestByIdQueryHandler> _logger;

        public GetRequestByIdQueryHandler(
            IRequestRepository repo,
            ILogger<GetRequestByIdQueryHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<RequestBase?> Handle(
            GetRequestByIdQuery request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching request with ID {Id}", request.Id);

            var entity = await _repo.GetByIdAsync(request.Id);

            if (entity == null)
            {
                _logger.LogWarning("Request with ID {Id} not found", request.Id);
            }

            return entity;
        }
    }
}
