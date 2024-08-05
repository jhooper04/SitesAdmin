using Microsoft.OpenApi.Models;
using SitesAdmin.Features.Assets.Dto;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

public class FileUploadOperation : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var uploadFileMediaType = "multipart/form-data";

        var fileUploadParams = context.MethodInfo.GetParameters()
            .Where(p => p.ParameterType == typeof(UploadRequest))
            .ToList();

        if (fileUploadParams.Any())
        {
            operation.RequestBody = new OpenApiRequestBody
            {
                Content = {
                    [uploadFileMediaType] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "object",
                            Properties = {
                                ["file"] = new OpenApiSchema
                                {
                                    Type = "string",
                                    Format = "binary"
                                },
                                ["caption"] = new OpenApiSchema
                                {
                                    Type = "string"
                                },
                                ["description"] = new OpenApiSchema
                                {
                                    Type = "string"
                                },
                                ["accessRoles"] = new OpenApiSchema
                                {
                                    Type = "string"
                                },
                                ["imageWidth"] = new OpenApiSchema
                                {
                                    Type = "integer"
                                },
                                ["imageHeight"] = new OpenApiSchema
                                {
                                    Type = "integer"
                                },
                                ["generateThumbnails"] = new OpenApiSchema
                                {
                                    Type = "boolean"
                                },
                                ["folderId"] = new OpenApiSchema
                                {
                                    Type = "integer"
                                }
                            },
                            Required = new HashSet<string> { }
                        }
                    }
                }
            };
        }
    }
}
