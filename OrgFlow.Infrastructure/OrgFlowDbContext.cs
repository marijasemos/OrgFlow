using Microsoft.EntityFrameworkCore;
using OrgFlow.Domain.Entites;
using OrgFlow.Domain.Entities;


namespace OrgFlow.Infrastructure
{
    public class OrgFlowDbContext : DbContext
    {
        public OrgFlowDbContext(DbContextOptions<OrgFlowDbContext> options) : base(options)
        {
        }

        public DbSet<Organization> Organizations => Set<Organization>();
        public DbSet<User> Users => Set<User>();
        public DbSet<RequestBase> Requests => Set<RequestBase>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();
        public DbSet<RequestType> RequestTypes => Set<RequestType>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Organization>()
                .HasMany(o => o.Users)
                .WithOne(u => u.Organization)
                .HasForeignKey(u => u.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Organization>()
                .HasMany(o => o.Requests)
                .WithOne(r => r.Organization)
                .HasForeignKey(r => r.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Roles)
                .WithOne(r => r.User!)
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.CreatedRequests)
                .WithOne(r => r.CreatedBy)
                .HasForeignKey(r => r.CreatedByUserId);

            // seed
            modelBuilder.Entity<Organization>().HasData(new Organization { Id = 1, Name = "Cyclomedia", IsActive = true });
            modelBuilder.Entity<User>().HasData(new User { Id = 1, Username = "marija", Email = "marija@example.com", PasswordHash = "dummy", OrganizationId = 1 });


        }

    }
}
