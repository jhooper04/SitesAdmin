
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SitesAdmin.Data;
using SitesAdmin.Features.Identity;
using System.Text.Json.Serialization;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Ardalis.GuardClauses;
using System.Reflection;
using SitesAdmin.Services;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SitesAdmin.Data.Interceptors;
using Microsoft.AspNetCore.Authorization;
using SitesAdmin.Features.Identity.Services;
using SitesAdmin.Features.Identity.Handlers;
using SitesAdmin.Features.Identity.Interfaces;
using SitesAdmin.Features.Identity.Data;
using AutoMapper;
using Microsoft.AspNetCore.HttpOverrides;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace SitesAdmin
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var infoMessages = new List<(LogLevel Level, string Message)>();


            builder.Configuration.AddEnvironmentVariables("_APP_");
            builder.Configuration.AddCommandLine(args);

            

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(option =>
            {
                option.CustomSchemaIds(type => {
                    string name = type.Name ?? "";
                    var types = type.GetGenericArguments().Select(t=>t.Name);
                    name = string.Join("", types) + name;
                    name = name.Replace("`1", "")
                        .Replace(type.Namespace + ".", "")
                        .Replace("Response", "")
                        .Replace("Paginated", "");
                    return name;
                });
                option.SupportNonNullableReferenceTypes();
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Test API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
                option.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Enter a valid ApiKey",
                    Name = ApiKeyOrRoleAuthorizationHandler.ApiKeyHeaderName,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "ApiKeyScheme"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "ApiKey"
                            }
                        },
                        new string[] { }
                    }
                });
                option.OperationFilter<FileUploadOperation>();
            });

            builder.Services.AddProblemDetails();
            builder.Services.AddApiVersioning();
            builder.Services.AddRouting(options => options.LowercaseUrls = true);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");

            string[] envDbKeys = new string[] { "DB_SERVER", "DB_SCHEMA", "DB_USER", "DB_PASS" };

            foreach (var envDbKey in envDbKeys)
            {
                string? value = builder.Configuration.GetValue<string>(envDbKey);
                if (value == null) infoMessages.Add((LogLevel.Error, $"Missing {envDbKey} configuration for the database connection string"));

                connectionString = connectionString.Replace($"{{{envDbKey}}}", value);
            }

            builder.Services.AddScoped<IUser, CurrentUser>();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            builder.Services.AddSingleton<ISaveChangesInterceptor, SluggableSaveChangesInterceptor>();

            builder.Services.AddSingleton<IMaterializationInterceptor, SluggableMaterializeInterceptor>();

            builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                options
                    .AddInterceptors(sp.GetServices<ISaveChangesInterceptor>())
                    .AddInterceptors(sp.GetServices<IMaterializationInterceptor>());

                options.UseMySql(connectionString, new MySqlServerVersion(new Version(10, 7, 8)));
            });

            builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

            builder.Services.AddScoped<ApplicationDbContextInitialiser>();

            builder.Services.AddSingleton<IApiKeyValidator, ApiKeyValidator>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<ITokenService, TokenService>();

            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

            

            builder.Services.AddSingleton(TimeProvider.System);

            // Support string to enum conversions
            builder.Services.AddControllers().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });


            // Specify identity requirements
            // Must be added before .AddAuthentication otherwise a 404 is thrown on authorized endpoints
            builder.Services
                .AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();


            // These will eventually be moved to a secrets file, but for alpha development appsettings is fine
            var validIssuer = builder.Configuration.GetValue<string>("BASE_URL"); 
            var validAudience = builder.Configuration.GetValue<string>("AUDIENCE"); 
            var symmetricSecurityKey = builder.Configuration.GetValue<string>("JWT_KEY");

            if (symmetricSecurityKey == null) infoMessages.Add((LogLevel.Error, $"Missing SymmetricSecurityKey configuration"));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.IncludeErrorDetails = true;
                    options.SaveToken = true;
                    //options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        
                        ClockSkew = TimeSpan.Zero,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = validIssuer,
                        ValidAudience = validAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(symmetricSecurityKey ?? "TestKey")
                        ),
                    };
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiKeyOrAdmin", policy =>
                    policy.Requirements.Add(new ApiKeyOrRoleRequirement(Role.Administrator)));
            });

            builder.Services.AddSingleton<IAuthorizationHandler, ApiKeyOrRoleAuthorizationHandler>();

            var corsPolicy = "_SitesAdminApp";
            List<string> allowedOrigins = new List<string> { validIssuer ?? "", validAudience ?? "" };
            

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(
                    corsPolicy,
                    policy => policy.WithOrigins(allowedOrigins.ToArray())
                    .AllowAnyMethod()
                    .SetIsOriginAllowed(isOriginAllowed => true)
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithExposedHeaders("Www-Authenticate"));
            });

            // Build the app
            var app = builder.Build();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            var mapper = app.Services.GetRequiredService<IMapper>();
            mapper.ConfigurationProvider.AssertConfigurationIsValid();

            app.UseCors(corsPolicy);

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            await app.InitialiseDatabaseAsync();

            //app.UseHttpsRedirection();
            app.UseStatusCodePages();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.MapControllers();

            if (infoMessages.Count > 0)
            {
                ILogger logger = app.Services.GetRequiredService<ILogger<Program>>();
                infoMessages.ForEach(log => logger.Log(log.Level, log.Message));
            }

            app.Run();
        }
    }
}
