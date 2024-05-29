using Microsoft.AspNetCore.Authorization;

namespace SitesAdmin.Features.Identity
{
    public class ApiKeyOrRoleRequirement : IAuthorizationRequirement
    {
        public string Role { get; }
        
        public ApiKeyOrRoleRequirement(string role)
        {
            Role = role; 
        }
    }

    public class ApiKeyOrRoleAuthorizationHandler : AuthorizationHandler<ApiKeyOrRoleRequirement>
    {
        public const string ApiKeyHeaderName = "X-Api-Key";
        private readonly IApiKeyValidator _apiKeyValidator;
        private readonly IHttpContextAccessor _contextAccessor;

        public ApiKeyOrRoleAuthorizationHandler(IApiKeyValidator apiKeyValidator, IHttpContextAccessor contextAccessor)
        {
            _apiKeyValidator = apiKeyValidator;
            _contextAccessor = contextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ApiKeyOrRoleRequirement requirement)
        {
            if (_contextAccessor.HttpContext == null) return Task.CompletedTask;

            if (_contextAccessor.HttpContext.User.IsInRole(requirement.Role))
            {
                context.Succeed(requirement);
            }

            string? providedApiKey = _contextAccessor.HttpContext.Request.Headers[ApiKeyHeaderName];

            if (providedApiKey == null) return Task.CompletedTask;

            if (_apiKeyValidator.IsValid(providedApiKey))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
