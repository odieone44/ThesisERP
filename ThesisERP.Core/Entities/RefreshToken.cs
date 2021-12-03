using System.Security.Cryptography;

namespace ThesisERP.Core.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime Expires { get; set; }
        public DateTime Created { get; set; }
        public string CreatedByIp { get; set; } = string.Empty;
        public DateTime? Revoked { get; set; }
        public string? RevokedByIp { get; set; }
        public string? ReplacedByToken { get; set; }
        public string? ReasonRevoked { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public bool IsRevoked => Revoked != null;
        public bool IsActive => !IsRevoked && !IsExpired;

        public RefreshToken(string ipAddress)
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);

            Token = Convert.ToBase64String(randomBytes);
            Expires = DateTime.UtcNow.AddDays(7);
            Created = DateTime.UtcNow;
            CreatedByIp = ipAddress;
        }

        public RefreshToken()
        { }

        public void Revoke(string ipAddress, string reason, string? replacementToken = null)
        {
            Revoked = DateTime.UtcNow;
            RevokedByIp = ipAddress;
            ReasonRevoked = reason;
            ReplacedByToken = replacementToken;
        }

        public static RefreshToken RotateRefreshToken(RefreshToken refreshToken, string ipAddress)
        {
            var newRefreshToken = new RefreshToken(ipAddress);

            refreshToken.Revoke(ipAddress, "Replaced By New Token", newRefreshToken.Token);

            return newRefreshToken;
        }
    }
}
