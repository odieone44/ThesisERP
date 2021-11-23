using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThesisERP.Core.Models
{
    public class JwtSettings
    {
        public string Issuer { get; set; }
        public int Lifetime { get; set; }
        public int RefreshTokenTTL { get; set; }
    }
}
