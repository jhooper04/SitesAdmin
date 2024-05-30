namespace SitesAdmin.Features.Identity.Services
{
    public interface IApiKeyValidator
    {
        bool IsValid(string apiKey);
    }
    public class ApiKeyValidator : IApiKeyValidator
    {
        private readonly IConfiguration _configuration;
        public ApiKeyValidator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool IsValid(string apiKey)
        {
            // Implement logic for validating the API key.
            //var keys = _configuration.GetSection("ApiClientKeys").GetChildren();
            string[] keys = [_configuration.GetValue<string>("CLIENT_API_KEY") ?? ""];

            //string[] generatorKeys = _configuration.GetValue<string[]>("ApiClientKeys") ?? [];

            foreach (var key in keys)
            {
                if (apiKey == key)
                    return true;
            }
            return false;
        }
    }
}
