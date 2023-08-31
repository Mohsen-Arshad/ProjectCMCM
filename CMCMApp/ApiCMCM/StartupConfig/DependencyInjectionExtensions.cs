using LibraryCMCM.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ApiCMCM.StartupConfig;

public static class DependencyInjectionExtensions
{
    public static void AddStandardServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.AddSwaggerServices();
    }

    public static void AddSwaggerServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen();
    }

    public static void AddCustomServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
        builder.Services.AddSingleton<IUserData, UserData>();
        builder.Services.AddSingleton<IRequestData, RequestData>();
        builder.Services.AddSingleton<ICategoryData, CategoryData>();
        builder.Services.AddSingleton<IFileService, FileService>();
        builder.Services.AddSingleton<IDocumentData, DocumentData>();
    }

    public static void AddAuthenticationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization(opts =>
        {
            opts.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
        });

        builder.Services.AddAuthentication("Bearer").AddJwtBearer(opts =>
        {
            opts.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidIssuer = builder.Configuration.GetValue<string>("Authentication:Issuer"),
                ValidAudience = builder.Configuration.GetValue<string>("Authentication:Audience"),
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(
                        builder.Configuration.GetValue<string>("Authentication:SecretKey")!)),
                ClockSkew = TimeSpan.Zero
            };
        });
    }

    public static void AddCachingServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddResponseCaching();
    }

    public static void AddHealthCheckServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks();
    }
}
