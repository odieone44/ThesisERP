﻿using Microsoft.AspNetCore.Identity;

namespace ThesisERP.Core.Entities
{
    public class AppUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

        public RefreshToken AddRefreshToken(string ipAddress)
        {
            RefreshToken refreshToken = new(ipAddress);

            RefreshTokens.Add(refreshToken);

            return refreshToken;
        }

        public void RemoveExpiredRefreshTokens(int refreshTokenDaysToLive)
        {
           RefreshTokens = RefreshTokens
                            .Where(token=> 
                                    token.Created.AddDays(refreshTokenDaysToLive) > DateTime.UtcNow)
                            .ToList();              
        }

        public void RevokeAllDescendantRefreshTokens(RefreshToken refreshToken, string ipAddress, string reason)
        {
            if (!string.IsNullOrEmpty(refreshToken.ReplacedByToken))
            {
                var childToken = RefreshTokens.SingleOrDefault(x => x.Token == refreshToken.ReplacedByToken);
                if (childToken.IsActive)
                    childToken.Revoke(ipAddress: ipAddress, reason: reason);
                else
                    RevokeAllDescendantRefreshTokens(childToken, ipAddress, reason);
            }
        }
    }
}