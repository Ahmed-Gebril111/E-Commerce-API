using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.APIs.Helpers;
using Talabat.APIs.Middlwares;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repository;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            #region configure services Add services to the container.


            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddDbContext<AppIdentityDbContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            builder.Services.AddSingleton<IConnectionMultiplexer>(Options =>
            {
                var Connection = builder.Configuration.GetConnectionString("RedisConnection");
                return ConnectionMultiplexer.Connect(Connection);
            });



            builder.Services.AddApplicationServices();


            builder.Services.AddIdentityServices();


            #endregion




            var app = builder.Build();



            #region Update-DataBase
            using var Scope = app.Services.CreateScope();
            var Services = Scope.ServiceProvider;

            var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();

            try
            {
                var DbContext = Services.GetRequiredService<StoreContext>();
                await DbContext.Database.MigrateAsync();
                var IdentityDbContext = Services.GetRequiredService<AppIdentityDbContext>();
                await IdentityDbContext.Database.MigrateAsync();
                await StoreContextSeed.SeedAsync(DbContext);
                var UserManager = Services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedUserAsync(UserManager);
                await StoreContextSeed.SeedAsync(DbContext);
            }
            catch (Exception ex)
            {

                var Logger = LoggerFactory.CreateLogger<Program>();
                Logger.LogError(ex, "An Error Occured During Appling The Migration ");

            }

            #endregion


            #region Configure - Configure the HTTP request pipeline.
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMiddleware<ExceptionMiddleware>();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStatusCodePagesWithRedirects("/errors/{0}");

            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers(); 
            #endregion

            app.Run();
        }
    }
}