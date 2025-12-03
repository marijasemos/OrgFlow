using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OegFlow.Domain.DTOs;
using OrgFlow.Application.Interfaces;
using OrgFlow.Application.Organizations.Commands;
using OrgFlow.Application.Organizations.Queries;
using OrgFlow.Application.Services;
using OrgFlow.Domain.Entites;

namespace OrgFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]

    public class OrganizationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrganizationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET api/organizations
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _mediator.Send(new GetAllOrganizationsQuery());
            return Ok(items);
        }

        // GET api/organizations/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _mediator.Send(new GetOrganizationByIdQuery(id));
            if (item == null) return NotFound();
            return Ok(item);
        }

        // GET api/organizations/by-name/{name}
        [HttpGet("by-name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var item = await _mediator.Send(new GetOrganizationByNameQuery(name));
            if (item == null) return NotFound();
            return Ok(item);
        }

        // POST api/organizations
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrganizationDto dto)
        {
            var created = await _mediator.Send(new CreateOrganizationCommand(dto));
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT api/organizations/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateOrganizationDto dto)
        {
            if (id != dto.Id)
                return BadRequest("Id in URL and body must match.");

            var updated = await _mediator.Send(new UpdateOrganizationCommand(dto));
            return Ok(updated);
        }

        // DELETE (soft) api/organizations/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deactivate(int id)
        {
            await _mediator.Send(new DeactivateOrganizationCommand(id));
            return NoContent();
        }
    }
}