using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<OrgFlowDbContext>(options =>
       options.UseSqlServer(connectionString)
    );

builder.Services.Configure<ApprovalPolicySettings>(
        builder.Configuration.GetSection("ApprovalPolicies")
    );

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
builder.Services.AddScoped<IOrganizationService, OrganizationService>();
builder.Services.AddScoped<RequestWorkflowService>();
builder.Services.AddTransient<RequestValidator>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseGlobalExceptionHandling();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
