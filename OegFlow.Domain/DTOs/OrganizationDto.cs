using System.ComponentModel.DataAnnotations;

namespace OrgFlow.Api
{
    public class OrganizationDto
    {
        [Required]
        public string Name { get; set; }
    }
}
