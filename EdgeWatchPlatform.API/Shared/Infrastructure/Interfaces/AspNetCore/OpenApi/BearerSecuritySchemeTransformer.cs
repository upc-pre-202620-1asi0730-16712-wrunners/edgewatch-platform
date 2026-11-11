using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace EdgeWatchPlatform.API.Shared.Infrastructure.Interfaces.AspNetCore.OpenApi;

/// <summary>Adds the JWT bearer security scheme to the OpenAPI document so Scalar can send the Authorization header.</summary>
public sealed class BearerSecuritySchemeTransformer : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>
        {
            ["Bearer"] = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                In = ParameterLocation.Header,
                BearerFormat = "JWT",
                Description = "Enter the JWT bearer token"
            }
        };
        return Task.CompletedTask;
    }
}
