using LoginAuthenticationWithCustom.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LoginAuthenticationWithCustom
{
    public class CustomAuthorization
    {
        public static async Task InitializeData(IServiceProvider services, ILoggerFactory loggerFactory)
        {
            var logger = loggerFactory.CreateLogger("Custom Auth");

            using (var serviceScope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var env = serviceScope.ServiceProvider.GetService<IHostingEnvironment>();
                if (!env.IsDevelopment()) return;

                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                var adminTask = roleManager.CreateAsync(
                    new IdentityRole { Name = "Admin" });

                var powerUserTask = roleManager.CreateAsync(
                    new IdentityRole { Name = "PowerUser" });

                Task.WaitAll(adminTask, powerUserTask);
                logger.LogInformation("==> Added Admin role");

                var userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

                var user = new ApplicationUser
                {
                    Email = "daniele@irsuti.it",
                    UserName = "daniele@irsuti.it"
                };

                await userManager.CreateAsync(user, "Passw0rd!");
                logger.LogInformation("daniele@irsuti.it created");

                await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Country, "Italia"));
                logger.LogInformation("Pizza mafia mandolino, daniele è italiano adesso");

                await userManager.AddToRoleAsync(user, "PowerUser");
                logger.LogInformation("daniele@irsuti.it now is PowerUser");




            }
        }
    }
}