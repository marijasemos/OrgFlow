using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrgFlow.Application.Services;
using OrgFlow.Domain.DTOs;

namespace OrgFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, [FromServices] RequestWorkflowService workflow)
        {
            var request = await workflow.GetByIdAsync(id);

            if (request == null)
                return NotFound();

            return Ok(request);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromServices] RequestWorkflowService workflow, CreateRequestDto dto)
        {
            var request = await workflow.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = request.Id }, request);
        }

        [HttpPost("{id}/submit")]
        public async Task<IActionResult> Submit(int id, [FromServices] RequestWorkflowService workflow)
        {
            var request = await workflow.SubmitAsync(id);
            return Ok(request);
        }
    }
}
