using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using OegFlow.Domain;
using OrgFlow.Api.Middleware;
using OrgFlow.Application;
using OrgFlow.Application.Configuration;
using OrgFlow.Application.Helpers;
using OrgFlow.Application.Interfaces;
using OrgFlow.Application.Requests.Queries;
using OrgFlow.Application.Services;
using OrgFlow.Infrastructure;
using OrgFlow.Infrastructure.Interfaces;
using OrgFlow.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using OegFlow.Domain.Models;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<OrgFlowDbContext>(options =>
       options.UseSqlServer(connectionString)
    );

// 2. IDENTITY
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;
    })
    .AddEntityFrameworkStores<OrgFlowDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<ApprovalPolicySettings>(
        builder.Configuration.GetSection("ApprovalPolicies")
    );
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

//builder.Services.AddMediatR(cfg =>
//cfg.RegisterServicesFromAssembly(typeof(GetAllRequestsQuery).Assembly));    

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Marker).Assembly));

builder.Services.AddScoped<IApprovalPolicyProvaider, ApprovalPolicyProvider>();
builder.Services.AddTransient<IApprovalStrategy, LeaveApprovalStrategy>();
builder.Services.AddTransient<IApprovalStrategy, RemoteWorkApprovalStrategy>();

builder.Services.AddScoped<IApprovalStrategyResolver, ApprovalStrategyResolver>();
builder.Services.AddTransient<IRequestFactory, RequestFactory>();

builder.Services.AddScoped<IRequestRepository, RequestRepository>();
builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
builder.Services.AddScoped<RequestWorkflowService>();
builder.Services.AddTransient<RequestValidator>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<IOfficeLocationRepository, OfficeLocationRepository>();
builder.Services.AddScoped<IPositionRepository, PositionRepository>();
builder.Services.AddScoped<IPositionRepository, PositionRepository>();
builder.Services.AddScoped<IEmploymentContractRepository, EmploymentContractRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<UserContext>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection("Jwt"));

var jwtSection = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSection["Key"]!);
// Add authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSection["Issuer"],
        ValidAudience = jwtSection["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero
    };
    // Configure token validation parameters here
});

// Add Swagger with JWT support
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1",
        Description = "API with JWT Bearer authentication in Swagger"
    });

    // Define the BearerAuth scheme
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token."
    });

    // Apply BearerAuth globally
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                    {
                    new OpenApiSecurityScheme
                    {
                    Reference = new OpenApiReference
                    {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                    }
                    },
                    Array.Empty<string>()
                    }
                    });
                    });


// 2) Authentication + JWT Bearer
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    });
 
// 3) Authorization (može kasnije i policy-je)
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseGlobalExceptionHandling();

//app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseRouting();
app.Use(async (context, next) =>
{
    Console.WriteLine(" AUTH HEADER: " + context.Request.Headers["Authorization"]);
    await next();
});
//app.UseCors("Open");

app.UseAuthentication();   // obavezno pre UseAuthorization
app.UseMiddleware<UserContextMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
