using Microsoft.AspNetCore.Mvc;
using OegFlow.Domain.DTOs;
using OrgFlow.Application.Interfaces;
using OrgFlow.Application.Services;
using OrgFlow.Domain.Entites;

namespace OrgFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizationsController : ControllerBase
    {
        public readonly IOrganizationService _organizationService = new OrganizationService();

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Organization>>> GetAll()
        {
            var result = await _organizationService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<OrganizationPaginated>> GetAllPaginated([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _organizationService.GetPaginatedAsync(page, pageSize);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Organization>> Get(int id)
        {
            var result = await _organizationService.GetByIdAsync(id);
            if (result is null)
                return NotFound(); // slajd 16

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Organization>> Create([FromBody] OrganizationDto dto)
        {
            var result = await _organizationService.CreateAsync(dto);

            // slajd 16: 201 Created + Location
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }
    }
}