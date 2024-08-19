
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

public class MarkRequiredNonNullableSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema?.Properties == null || schema.Properties.Count == 0 || context.Type == null)
            return;

        // Loop through each property in the schema
        foreach (var property in schema.Properties)
        {
            // Get the corresponding property info from the .NET type
            var propertyInfo = context.Type.GetProperties().FirstOrDefault(x => string.Equals(x.Name, property.Key, StringComparison.OrdinalIgnoreCase));

            // Check if the property is non-nullable and mark it as required
            if (propertyInfo != null && !IsNullable(propertyInfo.PropertyType))
            {
                schema.Required.Add(property.Key);
            }
        }
    }

    private bool IsNullable(Type type)
    {
        // Check for nullable types
        return Nullable.GetUnderlyingType(type) != null || !type.IsValueType;
    }
}