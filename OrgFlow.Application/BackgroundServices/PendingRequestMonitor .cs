using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrgFlow.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgFlow.Application.BackgroundServices
{
    public class PendingRequestMonitor : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<PendingRequestMonitor> _logger;

        public PendingRequestMonitor(
            IServiceScopeFactory scopeFactory,
            ILogger<PendingRequestMonitor> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("PendingRequestMonitor started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var repo = scope.ServiceProvider.GetRequiredService<IRequestRepository>();

                    var stale = await repo.GetStalePendingRequestsAsync(stoppingToken);

                    foreach (var req in stale)
                    {
                        _logger.LogWarning(
                            "Request {Id} pending more than 2h. Created: {CreatedAt}",
                            req.Id, req.CreatedAt);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in PendingRequestMonitor");
                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }

            _logger.LogInformation("PendingRequestMonitor stopped.");
        }
    }

}
