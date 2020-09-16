using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using StandardApi.Constants.System;
using StandardApi.Core.Auth.Queries.GetInfoFromToken;
using StandardApi.Core.Auth.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace StandardApi.Core.Auth.Queries.GetCurrentUser
{
    public class GetCurrentUserQuery : IGetCurrentUserQuery
    {
        private readonly IGetInfoFromTokenQuery _getInfoFromTokenQuery;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetCurrentUserQuery(IGetInfoFromTokenQuery getInfoFromTokenQuery, IHttpContextAccessor httpContextAccessor)
        {
            _getInfoFromTokenQuery = getInfoFromTokenQuery;
            _httpContextAccessor = httpContextAccessor;
        }

        public int UserId
        {
            get
            {
                var header = _httpContextAccessor.HttpContext.Request.Headers;
                var token = ((string)header[HeaderNames.Authorization])?.Substring("Bearer ".Length).Trim();
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var sUserId = jwtToken.Claims.First(claim => claim.Type == ClaimType.USER_ID).Value;

                int iUserId = 0;
                int.TryParse(sUserId, out iUserId);

                return iUserId;
            }
        }

        public async Task<UserTokenInfoModel> ExecuteAsync()
        {
            //var authInfo = await _httpContextAccessor.HttpContext.Authentication.GetAuthenticateInfoAsync(OAuthValidationDefaults.AuthenticationScheme);

            var ass = _httpContextAccessor.HttpContext.User.Identity;


            var header = _httpContextAccessor.HttpContext.Request.Headers;
            var token = ((string)header[HeaderNames.Authorization])?.Substring("Bearer ".Length).Trim();

            return await _getInfoFromTokenQuery.ExecuteAsync(token);
        }
    }
}
