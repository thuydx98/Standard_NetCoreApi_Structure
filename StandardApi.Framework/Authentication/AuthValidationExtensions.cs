using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace StandardApi.Framework.Authentication
{
    public static class AuthValidationExtensions
    {
        public static AuthenticationBuilder UseAuthValidation(this AuthenticationBuilder app, string authenticationScheme, Action<AuthValidationOptions> options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return app.AddScheme<AuthValidationOptions, AuthValidationHandler>(authenticationScheme, options);
        }
    }
}
