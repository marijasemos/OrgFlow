using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OegFlow.Domain.Models;

namespace OrgFlow.Application.Interfaces
{
    public interface IAuthService
    {

            Task<(bool IsSuccess, string Message)> RegisterAsync(RegisterRequest request, string role);
            Task<(bool IsSuccess, string Token)> LoginAsync(LoginRequest request);
        }
    }

