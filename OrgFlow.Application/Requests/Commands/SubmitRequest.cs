using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using OrgFlow.Application.Interfaces;
using OrgFlow.Domain.Entities;
using OrgFlow.Domain.Enums;
using OrgFlow.Infrastructure.Interfaces;

namespace OrgFlow.Application.Requests.Commands
{
    public record SubmitRequestCommand(int RequestId) : IRequest<RequestBase>;
    public class SubmitRequestCommandHandler
    : IRequestHandler<SubmitRequestCommand, RequestBase>
    {
        private readonly IRequestRepository _repo;
        private readonly IApprovalStrategyResolver _strategyResolver;

        public SubmitRequestCommandHandler(
            IRequestRepository repo,
            IApprovalStrategyResolver strategyResolver)
        {
            _repo = repo;
            _strategyResolver = strategyResolver;
        }

        public async Task<RequestBase> Handle(
            SubmitRequestCommand request,
            CancellationToken cancellationToken)
        {
            var entity = await _repo.GetByIdAsync(request.RequestId)
                ?? throw new KeyNotFoundException($"Request {request.RequestId} not found.");

            if (entity.Status != RequestStatus.Draft)
                throw new InvalidOperationException("Only draft requests can be submitted.");

            entity.Status = RequestStatus.Submitted;

            var strategy = _strategyResolver.Resolve(entity);
            await strategy.ApplyAsync(entity);

            await _repo.UpdateAsync(entity);

            return entity;
        }
    }
 }
