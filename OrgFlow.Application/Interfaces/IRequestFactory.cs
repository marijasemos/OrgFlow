using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrgFlow.Domain.DTOs;
using OrgFlow.Domain.Entities;

namespace OrgFlow.Application.Interfaces
{
    public interface IRequestFactory
    {
        RequestBase Create(CreateRequestDto dto);
    }
}
