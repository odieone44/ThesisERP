using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThesisERP.Application.Models
{
    public class AuthSuccess : AuthResponse
    {
        public AuthSuccess(string jwtToken, string refreshToken) : base(jwtToken, refreshToken) { }
    }
}
