using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;
using Talabat.Repository.Identity;
using Talabat.Service;

namespace Talabat.APIs.Extensions
{
    public static class IdentityServicesExtension
    {

        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenService,TokenService>();
           services.AddIdentity<AppUser, IdentityRole>(Options =>
            {
                Options.Password.RequireNonAlphanumeric = true;
                Options.Password.RequireDigit = true;
                Options.Password.RequireLowercase = true;
                Options.Password.RequireUppercase = true;

            })
                           .AddEntityFrameworkStores<AppIdentityDbContext>();
            services.AddAuthentication();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer();

            return services;
        }

    }
}
