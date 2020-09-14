using System;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace StandardApi.Framework.Configuration
{
    public static class ConfigurationExtensions
    {
        public static string GetDbConnectionString(this IConfiguration configuration, string enviroment)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (enviroment.ToLower() == "production")
            {
                return configuration["ConnectionStrings:DbConnection"];
            }

            return configuration["ConnectionStrings:DevConnection"];
        }

        public static string GetHangfireConnectionString(this IConfiguration configuration, string enviroment)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (enviroment.ToLower() == "production")
            {
                return configuration["ConnectionStrings:HangfireConnection"];
            }

            return configuration["ConnectionStrings:HangfireDevConnection"];
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
