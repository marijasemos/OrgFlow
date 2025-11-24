using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrgFlow.Application.Interfaces;
using OrgFlow.Domain.Entities;

namespace OrgFlow.Application.Services
{
    public class ApprovalStrategyResolver : IApprovalStrategyResolver
    {
        private readonly IEnumerable<IApprovalStrategy> _strategies;

        public ApprovalStrategyResolver(IEnumerable<IApprovalStrategy> strategies)
        {
            _strategies = strategies;
        }

        public IApprovalStrategy Resolve(RequestBase request)
        {
            if(request == null) 
                throw new ArgumentNullException(nameof(request));

            var strategy = _strategies.FirstOrDefault(s => s.CanHandle(request));

            if(strategy is null)
                throw new InvalidOperationException(
                    $"No approval strategy found for request type {request.GetType().Name}");

            return strategy;
        }
    }
}
