using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ReservasTucson.Authentication.Implementations;
using ReservasTucson.Authentication.Interfaces;
using ReservasTucson.Authentication.Services;
using ReservasTucson.Authentication.Support.Helpers;
using System.Text;

public static class Setup
{
    public static IServiceCollection AddCustomizedAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtConfig>(configuration.GetSection("JwtConfig"));

        services.AddHttpContextAccessor();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<ITokenService, TokenService>();

        // Usar SIEMPRE la misma config:
        var jwtConfig = configuration.GetSection("JwtConfig").Get<JwtConfig>();

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret));

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,

                ValidateIssuer = true,
                ValidIssuer = jwtConfig.Issuer,

                ValidateAudience = true,
                ValidAudience = jwtConfig.Audience,

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        return services;
    }
}

