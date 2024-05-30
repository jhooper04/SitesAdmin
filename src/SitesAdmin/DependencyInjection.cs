using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SitesAdmin.Data;
using SitesAdmin.Features.Identity.Data;
using System.Text.Json.Serialization;
using System.Text;
using System.Reflection;
using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using SitesAdmin.Services;

namespace SitesAdmin
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            //services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            //services.AddMediatR(cfg => {
            //    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            //    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            //    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
            //    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            //    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
            //});

            //var mapperConfig = new MapperConfiguration(mc =>
            //{
            //    mc.AddProfile(new MessageMappingProfile());
            //});

            //IMapper mapper = mapperConfig.CreateMapper();
            //services.AddSingleton(mapper);

            return services;
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");

            string[] envDbKeys = new string[] { "DB_SERVER", "DB_SCHEMA", "DB_USER", "DB_PASS" };

            foreach (var envDbKey in envDbKeys)
            {
                connectionString = connectionString.Replace($"{{{envDbKey}}}", configuration.GetValue<string>(envDbKey) ?? throw new InvalidOperationException("Missing connection string configuration"));
            }

            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

                options.UseMySQL(connectionString);
            });

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

            services.AddScoped<ApplicationDbContextInitialiser>();

            services
                .AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddApiEndpoints();


            return services;
        }

        public static IServiceCollection AddWebApiServices(this IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration)
        {
            //services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(option =>
            {
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
            });

            services.AddProblemDetails();
            services.AddApiVersioning();
            services.AddRouting(options => options.LowercaseUrls = true);

            // Register our TokenService dependency
            services.AddScoped<TokenService, TokenService>();

            // Support string to enum conversions
            services.AddControllers().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });


            // Specify identity requirements
            // Must be added before .AddAuthentication otherwise a 404 is thrown on authorized endpoints
            services
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
            var validIssuer = configuration.GetValue<string>("JwtTokenSettings:ValidIssuer");
            var validAudience = configuration.GetValue<string>("JwtTokenSettings:ValidAudience");
            var symmetricSecurityKey = configuration.GetValue<string>("JwtTokenSettings:SymmetricSecurityKey") ?? throw new Exception("Invalid Symetric Key");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.IncludeErrorDetails = true;
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
                            Encoding.UTF8.GetBytes(symmetricSecurityKey)
                        ),
                    };
                });

            return services;
        }
    }
}
