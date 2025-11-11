using OrgFlow.Domain.Entities;

namespace OrgFlow.Domain.Entites
{
    public class Organization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<User> Users { get; set; }
        public ICollection<RequestBase> Requests { get; set; }
    }
}
