using Microsoft.AspNetCore.Authentication;

namespace StandardApi.Framework.Authentication
{
    public class AuthValidationOptions : AuthenticationSchemeOptions
    {
        public string LoginPath { get; set; }
        public string ReturnUrlParameter { get; set; } = "ReturlUrl";
    }
}
