using Demo.DAL.Data;
using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		public UserController(UserManager<ApplicationUser> userManager)
        {
			_userManager = userManager;
		}
        

        public async Task<IActionResult> Index(string SearchValue="")
		{
			List<ApplicationUser> users;
			if (string.IsNullOrEmpty(SearchValue))
				users = await _userManager.Users.ToListAsync();
			else
				users=await _userManager.Users.Where(user=>user.Email.Trim().ToLower().Contains(SearchValue.ToLower())).ToListAsync();

            return View(users);
			
		}
        public async Task<IActionResult> Details(string id,string ViewName= "Details")
        {
            if (id == null)
            {
                return BadRequest();
            }
            var user =await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(ViewName, user);
        }
        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, ApplicationUser applicationUser)
        {
          
            if (id != applicationUser.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(id);
                    user.UserName = applicationUser.UserName;
                    user.NormalizedUserName = applicationUser.UserName.ToUpper();
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }
            return View(applicationUser);

        }
        public async Task<IActionResult> Delete([FromRoute] string id)
        {

            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                    return NotFound();

                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }

    }
 
}
