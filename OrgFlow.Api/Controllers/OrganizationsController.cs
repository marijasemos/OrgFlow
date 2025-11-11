using Microsoft.AspNetCore.Mvc;
using OrgFlow.Domain.Entites;

namespace OrgFlow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizationsController : ControllerBase
    {
        // za demo ćemo se praviti da nam je ovo “baza”
        private static readonly List<Organization> _orgs = new()
    {
        new Organization { Id = 1, Name = "OrgFlow" },
        new Organization { Id = 2, Name = "Cyclo Media" }
    };

        [HttpGet]
        public ActionResult<IEnumerable<Organization>> GetAll()
        {
            return Ok(_orgs);
        }

        [HttpGet]
        [Route("GetAll")]
        public ActionResult GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = _orgs.AsQueryable();

            var total = query.Count();
            var items = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var response = new
            {
                data = items,
                pagination = new
                {
                    currentPage = page,
                    pageSize = pageSize,
                    totalItems = total,
                    totalPages = (int)Math.Ceiling(total / (double)pageSize)
                }
            };

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public ActionResult<Organization> Get(int id)
        {
            var org = _orgs.FirstOrDefault(x => x.Id == id);
            if (org is null)
                return NotFound(); // slajd 16

            return Ok(org);
        }

        [HttpPost]
        public ActionResult<Organization> Create([FromBody] OrganizationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // slajd 24

            var newOrg = new Organization
            {
                Id = _orgs.Max(x => x.Id) + 1,
                Name = dto.Name
            };

            _orgs.Add(newOrg);

            // slajd 16: 201 Created + Location
            return CreatedAtAction(nameof(Get), new { id = newOrg.Id }, newOrg);
        }
    }
}