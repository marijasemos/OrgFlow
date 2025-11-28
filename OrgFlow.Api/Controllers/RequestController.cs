using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrgFlow.Application.Requests.Commands;
using OrgFlow.Application.Requests.Queries;
using OrgFlow.Application.Services;
using OrgFlow.Domain.DTOs;

namespace OrgFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RequestController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllRequestsQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var request = await _mediator.Send(new GetRequestByIdQuery(id));

            if (request == null)
                return NotFound();

            return Ok(request);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRequestDto dto)
        {
            var result = await _mediator.Send(new CreateRequestCommand(dto));

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPost("{id}/submit")]
        public async Task<IActionResult> Submit(int id, [FromServices] RequestWorkflowService workflow)
        {
            var result = await _mediator.Send(new SubmitRequestCommand(id));
            return Ok(result);
        }
    }
}
