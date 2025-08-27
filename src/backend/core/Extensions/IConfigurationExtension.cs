using Microsoft.Extensions.Configuration;

namespace core.Extensions
{
    public static class IConfigurationExtension
    {
        public static string GetJwtKeyIfExists(this IConfiguration configuration)
        {
            var jwtKey = configuration["JWT:key"];

            if (jwtKey is null)
            {
                throw new Exception("JWT-Key not found");
            }

            return jwtKey;
        }
    }
}
