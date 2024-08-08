using AhCh.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AhCh.Extensions;

public static class ApplicationConfigurationExtensions
{
    public static void AddAhChDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AhChContext>(Options =>
            Options.UseSqlServer(connectionString: configuration.GetConnectionString("mssql")));
    }
    public static void AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Security:Key"]!)),
                        ValidIssuer = configuration["Security:issuer"]!,
                        ValidAudience = configuration["Security:issuer"]!
                    };
                });
    }
}