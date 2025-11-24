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
        public DbSet<LeaveRequest> LeaveRequests => Set<LeaveRequest>();
        public DbSet<RemoteWorkRequest> RemoteWorkRequests => Set<RemoteWorkRequest>();
        public DbSet<UserRole> UserRoles => Set<UserRole>();

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

            var request = modelBuilder.Entity<RequestBase>();

            request.ToTable("Requests");

            request.HasKey(r => r.Id);

            // enum kao int
            request.Property(r => r.RequestType)
                   .HasConversion<int>();

            request.Property(r => r.Status)
                   .HasConversion<int>();


            // LeaveRequest specifična polja – mogu ostati sa istim imenima
            modelBuilder.Entity<LeaveRequest>(b =>
            {
                b.Property(l => l.From);
                b.Property(l => l.To);
                b.Property(l => l.Reason);
            });

            // RemoteWorkRequest specifična polja
            modelBuilder.Entity<RemoteWorkRequest>(b =>
            {
                b.Property(r => r.RemoteDate);
                b.Property(r => r.Location);
            });

            // Relacije 
            request
                .HasOne(r => r.Organization)
                .WithMany(o => o.Requests)
                .HasForeignKey(r => r.OrganizationId);

            request
                .HasOne(r => r.CreatedBy)
                .WithMany(u => u.CreatedRequests)
                .HasForeignKey(r => r.CreatedByUserId);

            // seed
            modelBuilder.Entity<Organization>().HasData(new Organization { Id = 1, Name = "Cyclomedia", IsActive = true });
            modelBuilder.Entity<User>().HasData(new User { Id = 1, Username = "marija", Email = "marija@example.com", PasswordHash = "dummy", OrganizationId = 1 });

        }

    }
}
