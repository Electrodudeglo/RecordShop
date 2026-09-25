using System.Text;

namespace RecordShop.Options
{
    public class JwtOptions
    {
        public const string SectionName = "Jwt";

        // Placeholder keys that ship with the repo. They must never sign tokens outside Development.
        public static readonly string[] PlaceholderKeys =
        {
            "dev-only-placeholder-key-change-me-0123456789",
            "REPLACE_WITH_A_LONG_RANDOM_SECRET_AT_LEAST_32_CHARS"
        };

        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;

        // Returns a list of problems with the settings; empty means they are valid.
        public List<string> Validate(bool isDevelopment)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(Issuer)) errors.Add("Jwt:Issuer is missing.");
            if (string.IsNullOrWhiteSpace(Audience)) errors.Add("Jwt:Audience is missing.");

            if (Encoding.UTF8.GetByteCount(Key) < 32)
                errors.Add("Jwt:Key is missing or shorter than 32 bytes.");
            else if (!isDevelopment && PlaceholderKeys.Contains(Key))
                errors.Add("Jwt:Key is still a placeholder. Set a real key (e.g. the Jwt__Key environment variable).");

            return errors;
        }
    }
}
