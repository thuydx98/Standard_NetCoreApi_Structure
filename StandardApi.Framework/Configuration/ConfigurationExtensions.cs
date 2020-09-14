using System;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace StandardApi.Framework.Configuration
{
    public static class ConfigurationExtensions
    {
        public static string GetDbConnectionString(this IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            return configuration["ConnectionStrings:DbConnection"];
        }

        public static string GetHangfireConnectionString(this IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            return configuration["ConnectionStrings:HangfireConnection"];
        }

        public static string[] GetAllowOrigins(this IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            return configuration["Auth:AllowOrigins"].Split(',').Select(val => val.Trim()).ToArray();
        }

        public static string GetLoginPath(this IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            return configuration["Auth:LoginPath"];
        }

        public static string GetLogoutPath(this IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            return configuration["Auth:LogoutPath"];
        }
    }
}
