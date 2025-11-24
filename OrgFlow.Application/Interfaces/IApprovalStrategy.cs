using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrgFlow.Domain.Entities;

namespace OrgFlow.Application.Interfaces
{
    public interface IApprovalStrategy
    {
        bool CanHandle(RequestBase request);
        Task ApplyAsync(RequestBase request);
    }
}
