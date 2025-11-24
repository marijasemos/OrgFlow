using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OrgFlow.Application.Helpers;
using OrgFlow.Application.Interfaces;
using OrgFlow.Domain.DTOs;
using OrgFlow.Domain.Entities;
using OrgFlow.Domain.Enums;
using OrgFlow.Infrastructure.Interfaces;

namespace OrgFlow.Application.Services
{
    public class RequestWorkflowService
    {
        private readonly IRequestRepository _repo;
        private readonly IRequestFactory _factory;
        private readonly IApprovalStrategyResolver _strategyResolver;
        private readonly RequestValidator _validator;
        private readonly ILogger<RequestWorkflowService> _logger;

        public RequestWorkflowService(
            IRequestRepository repo,
            IRequestFactory factory,
            IApprovalStrategyResolver strategyResolver,
            RequestValidator validator,
            ILogger<RequestWorkflowService> logger)
        {
            _repo = repo;
            _factory = factory;
            _strategyResolver = strategyResolver;
            _validator = validator;
            _logger = logger;
        }

        // ------------------------------------------
        // CREATE PIPELINE (EF)
        // ------------------------------------------
        public async Task<RequestBase> CreateAsync(CreateRequestDto dto)
        {
            _logger.LogInformation("Starting CREATE workflow (EF) for type {Type}", Enum.GetName(typeof(RequestType), dto.RequestTypeId));

            var dtoErrors = _validator.ValidateDto(dto);
            if (dtoErrors.Any())
                throw new ArgumentException(string.Join("; ", dtoErrors));

            var entity = _factory.Create(dto);

            var entityErrors = _validator.ValidateEntity(entity);
            if (entityErrors.Any())
                throw new ArgumentException(string.Join("; ", entityErrors));

            await _repo.AddAsync(entity);

            return entity;
        }

        // ------------------------------------------
        // SUBMIT PIPELINE (EF)
        // ------------------------------------------
        public async Task<RequestBase> SubmitAsync(int id)
        {
            _logger.LogInformation("Starting SUBMIT workflow  for Request {Id}", id);

            var request = await _repo.GetByIdAsync(id)
                          ?? throw new KeyNotFoundException($"Request {id} not found");

            if (request.Status != RequestStatus.Draft)
                throw new InvalidOperationException("Only draft requests can be submitted.");

            request.Status = RequestStatus.Submitted;

            var strategy = _strategyResolver.Resolve(request);
            await strategy.ApplyAsync(request);

            await _repo.UpdateAsync(request);

            return request;
        }

        public async Task<RequestBase> GetByIdAsync(int id)
        {
            var request = await _repo.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Request with ID {id} not found.");

            return request;
        }
    }
}
