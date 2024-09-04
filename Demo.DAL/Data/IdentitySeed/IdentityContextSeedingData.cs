using Demo.DAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Data.IdentityContextSeedingData
{
    public class IdentityContextSeedingData
    {
        public static async Task IdentitySeedAsync(UserManager<ApplicationUser> _userManager)
        {

            if (!_userManager.Users.Any())
            {
                var user = new ApplicationUser()
                {
                    Email = "shehabsameh987123@gmail.com",
                    PhoneNumber = "01147218434",
                    UserName = "User.Admin",
                    IsAgree = true,
                    FName = "user",
                    LName = "admin",
                };
                await _userManager.CreateAsync(user, "Shehab123@");
            }
        }
    }
}
