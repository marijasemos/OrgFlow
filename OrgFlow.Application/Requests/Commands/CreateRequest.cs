using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using OrgFlow.Application.Helpers;
using OrgFlow.Application.Interfaces;
using OrgFlow.Application.Services;
using OrgFlow.Domain.DTOs;
using OrgFlow.Domain.Entities;
using OrgFlow.Domain.Enums;
using OrgFlow.Infrastructure.Interfaces;

namespace OrgFlow.Application.Requests.Commands
{
    public record CreateRequestCommand(CreateRequestDto Dto) : IRequest<RequestBase>;

    public class CreateRequestCommandHandler : IRequestHandler<CreateRequestCommand, RequestBase>
    {
        private readonly IRequestRepository _repo;
        private readonly IRequestFactory _factory;
        private readonly RequestValidator _validator;
        private readonly ILogger<RequestWorkflowService> _logger;

        public CreateRequestCommandHandler(IRequestRepository repo,
            IRequestFactory factory, 
            RequestValidator validator,
             ILogger<RequestWorkflowService> logger)
        {
            _repo = repo;
            _factory = factory;
            _validator = validator;
            _logger = logger;
        }

        public async Task<RequestBase> Handle(CreateRequestCommand request, CancellationToken cancellationToken)
        {

            _logger.LogInformation("Starting CREATE workflow (EF) for type {Type}", Enum.GetName(typeof(RequestType), request.Dto.RequestTypeId));
            var dto = request.Dto;
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
    }
}
