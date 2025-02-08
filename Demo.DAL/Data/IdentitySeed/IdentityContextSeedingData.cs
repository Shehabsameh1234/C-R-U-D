using Demo.DAL.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.DAL.Data.IdentityContextSeedingData
{
    public class IdentityContextSeedingData
    {
        public static async Task IdentitySeedAsync(UserManager<ApplicationUser> _userManager , RoleManager<ApplicationRole> _roleManager)
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
                    IsVerified = true,
                };
                var role = new ApplicationRole()
                { 
                    Name ="Admin"
                };

                await _userManager.CreateAsync(user, "Shehab123@");
                await _roleManager.CreateAsync(role);
                await _userManager.AddToRoleAsync(user, role.Name);
            }
        }
    }
}
