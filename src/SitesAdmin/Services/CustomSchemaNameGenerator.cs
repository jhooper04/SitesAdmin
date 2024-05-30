using Microsoft.OpenApi.Models;
using NJsonSchema.Generation;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SitesAdmin.Services
{
    public class CustomSchemaNameGenerator : ISchemaNameGenerator
    {
        public string Generate(Type type)
        {
            return type?.FullName?.Replace("Response", "") ?? "";
        }
    }

    //public class SchemasVisibility : ISchemaFilter
    //{
    //    private readonly string[] VisibleSchemas = { "Schema1", "Schema2" };

    //    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    //    {
    //        foreach (var key in context.SchemaRepository.Schemas.Keys)
    //        {
    //            if (!VisibleSchemas.Contains(key))
    //                context.SchemaRepository.Schemas.Remove(key);
    //        }
    //    }
    //}
}
