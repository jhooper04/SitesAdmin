using NJsonSchema.Generation;

namespace SitesAdmin.Services
{
    public class CustomSchemaNameGenerator : ISchemaNameGenerator
    {
        public string Generate(Type type)
        {
            return type?.FullName?.Replace("Response", "") ?? "";
        }
    }
}
