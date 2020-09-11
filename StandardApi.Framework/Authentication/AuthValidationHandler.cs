using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using StandardApi.Common.Extentions;
using StandardApi.Common.Logger;
using StandardApi.Core.Auth.Queries.GetRoleByUserId;
using StandardApi.Core.Auth.Queries.GetInfoFromToken;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace StandardApi.Framework.Authentication
{
    public class AuthValidationHandler : AuthenticationHandler<AuthValidationOptions>
    {
        private const string AuthenticationSchema = "StandardApi.CookieSchema";
        private const string AuthenticationType = "StandardApi.CustomAuthenticationType";

        public AuthValidationHandler(IOptionsMonitor<AuthValidationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock) { }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var header = Request.Headers;
            var token = ((string)header[HeaderNames.Authorization])?.Substring("Bearer ".Length).Trim();
            var action = Request.Method;
            string controller = string.Empty;
            string api = string.Empty;

            try
            {
                controller = Request.Path.Value.Contains("/api/") ? Request.Path.Value.Split('/')[2] : string.Empty;
                api = Request.Path.Value.Contains("/api/") && Request.Path.Value.Split('/').Length > 3
                    ? Request.Path.Value.Split('/')[3]
                    : string.Empty;
            }
            catch (Exception)
            {
                Logging<AuthValidationHandler>.Warning("URL not valid: " + Request.Path.Value);
                return AuthenticateResult.Fail("URL not valid: " + Request.Path.Value);
            }

            AuthenticationTicket ticket = null;
            if (token.IsEmpty())
            {
                if (Request.Cookies["accessToken"] != null)
                {
                    token = Request.Cookies["accessToken"];
                    ticket = await CreateTicketAsync(token, action, controller, api);
                }
            }
            else
            {
                ticket = await CreateTicketAsync(token, action, controller, api);
            }

            if (ticket == null)
            {
                return AuthenticateResult.Fail("Authentication failed because the access token was invalid.");
            }

            //await Request.HttpContext.SignInAsync(ticket.AuthenticationScheme, ticket.Principal)

            await AuthenticationHttpContextExtensions.SignInAsync(Request.HttpContext, ticket.AuthenticationScheme, ticket.Principal);

            return AuthenticateResult.Success(ticket);
        }

        private async Task<AuthenticationTicket> CreateTicketAsync(string token, string action, string controller, string api)
        {
            string userName = string.Empty;
            try
            {
                var getUserFromToken = Request.HttpContext.RequestServices.GetService(typeof(IGetInfoFromTokenQuery)) as IGetInfoFromTokenQuery;
                var user = await getUserFromToken.ExecuteAsync(token);
                if (user == null)
                {
                    return null;
                }

                userName = user.UserName;

                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Id.ToString()),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim("FullName", user.FullName)
                    };

                var getRoleByUserIdQuery = Request.HttpContext.RequestServices.GetService(typeof(IGetRoleByUserIdQuery)) as IGetRoleByUserIdQuery;
                var roles = await getRoleByUserIdQuery.ExecuteAsync(user.Id);

                foreach (var role in roles)
                {
                    claims.Add(new Claim("Role", role.RoleName));
                }

                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, AuthenticationType));
                return new AuthenticationTicket(claimsPrincipal, new AuthenticationProperties(), AuthenticationSchema);
            }
            catch (Exception ex)
            {
                Logging<AuthValidationHandler>.Error(ex, "UserName: " + userName, "Action: " + action, "Controller: " + controller, "API: " + api);
                return null;
            }
        }
    }
}
