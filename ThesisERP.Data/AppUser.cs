using Microsoft.AspNetCore.Identity;

namespace ThesisERP.Data
{
    public class AppUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public List<RefreshToken> RefreshTokens { get; set; }
    }
}