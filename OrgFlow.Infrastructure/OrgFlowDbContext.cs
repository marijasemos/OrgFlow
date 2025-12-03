using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OegFlow.Domain.Entities;
using OegFlow.Domain.Models;
using OrgFlow.Domain.Entites;
using OrgFlow.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace OrgFlow.Infrastructure
{
    public class OrgFlowDbContext :  IdentityDbContext<ApplicationUser, IdentityRole, string>

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
        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Team> Teams => Set<Team>();
        public DbSet<OfficeLocation> OfficeLocations => Set<OfficeLocation>();
        public DbSet<Position> Positions => Set<Position>();
        public DbSet<EmploymentContract> EmploymentContracts => Set<EmploymentContract>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureOrganizationModel(modelBuilder);

            modelBuilder.Entity<EmploymentContract>(b =>
            {
                b.ToTable("EmploymentContracts");
                b.HasKey(ec => ec.Id);

                b.Property(ec => ec.ContractType)
                    .HasMaxLength(100)
                    .IsRequired();

                b.Property(ec => ec.Currency)
                    .HasMaxLength(10)
                    .IsRequired();

                b.HasOne(ec => ec.User)
                    .WithMany(u => u.EmploymentContracts)
                    .HasForeignKey(ec => ec.UserId);
            });
            //modelBuilder.Entity<Organization>()
            //    .HasMany(o => o.Users)
            //    .WithOne(u => u.Organization)
            //    .HasForeignKey(u => u.OrganizationId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //modelBuilder.Entity<Organization>()
            //    .HasMany(o => o.Requests)
            //    .WithOne(r => r.Organization)
            //    .HasForeignKey(r => r.OrganizationId)
            //    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>(b =>
            {
                b.ToTable("Users");
                b.HasKey(u => u.Id);

                b.Property(u => u.FirstName)
                    .HasMaxLength(100)
                    .IsRequired();

                b.Property(u => u.LastName)
                    .HasMaxLength(100)
                    .IsRequired();

                b.Property(u => u.Email)
                    .HasMaxLength(200)
                    .IsRequired();

                b.HasIndex(u => u.Email).IsUnique();
;

                b.HasOne(u => u.Department)
                    .WithMany(d => d.Users)
                    .HasForeignKey(u => u.DepartmentId)
                    .OnDelete(DeleteBehavior.SetNull);

                b.HasOne(u => u.Team)
                    .WithMany(t => t.Members)
                    .HasForeignKey(u => u.TeamId)
                    .OnDelete(DeleteBehavior.SetNull);

               

                b.HasOne(u => u.Manager)
                    .WithMany(m => m.DirectReports)
                    .HasForeignKey(u => u.ManagerId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            var request = modelBuilder.Entity<RequestBase>();

            request.ToTable("Requests");

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
            //request
            //    .HasOne(r => r.Organization)
            //    .WithMany(o => o.Requests)
            //    .HasForeignKey(r => r.OrganizationId);

            //request
            //    .HasOne(r => r.CreatedBy)
            //    .WithMany(u => u.CreatedRequests)
            //    .HasForeignKey(r => r.CreatedByUserId);

            // seed
            //modelBuilder.Entity<Organization>().HasData(new Organization { Id = 1, Name = "Cyclomedia", IsActive = true });
            //modelBuilder.Entity<User>().HasData(new User { Id = 1, Username = "marija", Email = "marija@example.com", PasswordHash = "dummy", OrganizationId = 1 });

        }

        private static void ConfigureOrganizationModel(ModelBuilder modelBuilder)
        {
            // Organization
            var org = modelBuilder.Entity<Organization>();

            org.ToTable("Organizations");
            org.HasKey(o => o.Id);

            org.Property(o => o.Name)
               .IsRequired()
               .HasMaxLength(200);

            // Department
            var dep = modelBuilder.Entity<Department>();

            dep.ToTable("Departments");
            dep.HasKey(d => d.Id);

            dep.Property(d => d.Name)
               .IsRequired()
               .HasMaxLength(200);

            dep.HasOne(d => d.Organization)
               .WithMany(o => o.Departments)
               .HasForeignKey(d => d.OrganizationId)
               .OnDelete(DeleteBehavior.Restrict);

            // Team
            var team = modelBuilder.Entity<Team>();

            team.ToTable("Teams");
            team.HasKey(t => t.Id);

            team.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(200);

            team.HasOne(t => t.Department)
                .WithMany(d => d.Teams)
                .HasForeignKey(t => t.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            team.HasOne(t => t.TeamLead)
                .WithMany()           // kasnije, ako želiš, možemo dodati User.TeamsLead
                .HasForeignKey(t => t.TeamLeadId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<User>(b =>
            {
                // MANAGER ↑ OVO JE KLJUČNO
                b.HasOne(u => u.Manager)
                    .WithMany(m => m.DirectReports)
                    .HasForeignKey(u => u.ManagerId)
                    .OnDelete(DeleteBehavior.Restrict); // or .NoAction()

                b.HasOne(u => u.Department)
                    .WithMany(d => d.Users)
                    .HasForeignKey(u => u.DepartmentId)
                    .OnDelete(DeleteBehavior.SetNull);

                b.HasOne(u => u.Team)
                    .WithMany(t => t.Members)
                    .HasForeignKey(u => u.TeamId)
                    .OnDelete(DeleteBehavior.SetNull);

                b.HasOne(u => u.Position)
                    .WithMany(p => p.Users)
                    .HasForeignKey(u => u.PositionId)
                    .OnDelete(DeleteBehavior.SetNull);

                b.HasOne(u => u.OfficeLocation)
                    .WithMany(o => o.Users)
                    .HasForeignKey(u => u.OfficeLocationId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }

}

