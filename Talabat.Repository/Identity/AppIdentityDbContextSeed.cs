﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any())
            {
                var User = new AppUser()
                {
                    DisplayName = "Ahmed Gebril",
                    Email = "Ahmed123@gmail.com",
                    PhoneNumber = "01098475345",
                    UserName = "AhmedGebril.Gboo"
                };
                await userManager.CreateAsync(User,"Pa$$w0rd");
            }
        
        
        
        }

    }
}
