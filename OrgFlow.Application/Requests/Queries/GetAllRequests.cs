using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using OrgFlow.Application.Interfaces;
using OrgFlow.Domain.Entities;
using OrgFlow.Infrastructure.Interfaces;

namespace OrgFlow.Application.Requests.Queries
{
    public record GetAllRequestsQuery() : IRequest<IReadOnlyList<RequestBase>>;

    public class GetAllRequestsQueryHandler
        : IRequestHandler<GetAllRequestsQuery, IReadOnlyList<RequestBase>>
    {
        private readonly IRequestRepository _repo;

        public GetAllRequestsQueryHandler(IRequestRepository repo)
        {
            _repo = repo;
        }

        public async Task<IReadOnlyList<RequestBase>> Handle(GetAllRequestsQuery request, 
            CancellationToken cancellationToken)
        {
            return await _repo.GetAllAsync();
        }
    }
}
