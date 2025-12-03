using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace OegFlow.Domain.Models
{
    public class RegisterRequest : IdentityUser
    {
        //public string Username { get; set; }
        //public string Password { get; set; }
        //public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
