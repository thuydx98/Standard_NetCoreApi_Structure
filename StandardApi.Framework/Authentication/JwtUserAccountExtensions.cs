using StandardApi.Constants.System;
using StandardApi.Framework.Configuration.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace StandardApi.Framework.Authentication
{
    public static class JwtUserAccountExtensions
    {
        private static long ToUnixEpochDate(DateTime date) => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        public static string GenerateToken(this JwtUserAccount account, JwtOptions jwtOptions)
        {
            try
            {
                jwtOptions.IssuedAt = DateTime.UtcNow;
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, account.UserId.ToString()),
                    new Claim(ClaimType.AUTHENTICATED, "True"),
                    new Claim(ClaimType.USERNAME, account.UserName ?? SystemConstant.EMPTY_USER_NAME),
                    new Claim(ClaimType.USER_ID, account.UserId.ToString()),
                    new Claim(ClaimType.USER_EMAIL, account.Email ?? SystemConstant.EMPTY_USER_EMAIL),
                    new Claim(ClaimType.USER_PHONE_NUMBER, account.PhoneNumber ?? SystemConstant.EMPTY_USER_PHONE_NUMBER),
                    new Claim(ClaimType.LAST_NAME, account.LastName),
                    new Claim(ClaimType.MIDDLE_NAME, account.MiddleName ?? SystemConstant.EMPTY_USER_MIDDLE_NAME),
                    new Claim(ClaimType.FIRST_NAME, account.FirstName),
                    new Claim(ClaimType.USER_CODE, account.UserCode ?? SystemConstant.EMPTY_USER_CODE),

                    new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64)
                };

                // Create the JWT and write it to a string
                JwtSecurityToken jwtToken = new JwtSecurityToken(
                    issuer: jwtOptions.Issuer,
                    audience: jwtOptions.Audience,
                    claims: claims,
                    notBefore: jwtOptions.NotBefore,
                    expires: jwtOptions.Expiration,
                    signingCredentials: jwtOptions.SigningCredentials);

                return new JwtSecurityTokenHandler().WriteToken(jwtToken);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
