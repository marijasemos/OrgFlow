using OrgFlow.Application.Interfaces;
using OrgFlow.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgFlow.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IRequestRepository _repo;

        public ReportService(IRequestRepository repo)
        {
            _repo = repo;
        }

        public async Task<byte[]> GenerateReportAsync(int requestId)
        {
            //  I/O-bound
            var request = await _repo.GetByIdAsync(requestId);

            if (request is null)
                throw new Exception("Request not found");

            //  CPU-bound
            return await Task.Run(() =>
            {
                // simulacija teške obrade
                Thread.Sleep(2000);

                return Encoding.UTF8.GetBytes(
                    $"Report for request {request.Id} generated at {DateTime.UtcNow}"
                );
            });
        }
    }

}
