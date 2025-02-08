using Demo.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

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
        

        public async Task<IActionResult> Index(string SearchInp )
		{
        
            List<ApplicationUser> users = new List<ApplicationUser>();
            if (string.IsNullOrEmpty(SearchInp))
            {
                ViewData["InnerBtn"] = "search";
                var allUsers = await _userManager.Users.ToListAsync();
                
                return View(allUsers);
            }
            else if (await _userManager.FindByEmailAsync(SearchInp)==null)
            {
                ViewData["InnerBtn"] = "search";
                ViewData["Message"] = "there is no user called " + SearchInp;

                var allUsers = await _userManager.Users.ToListAsync();

                return View(allUsers);
            }
            else
            {
                ViewData["InnerBtn"] = "Get All";
                var user =await _userManager.FindByEmailAsync(SearchInp);
                users.Add(user);
                return View(users);
            }

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
            var signedUser = await _userManager.GetUserAsync(User);
            if (signedUser.IsVerified == false)
            {
                TempData["delete"] = "Please Verify Your Email";
                return RedirectToAction(nameof(Index));
            }
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
            var signedUser = await _userManager.GetUserAsync(User);
            if (signedUser.IsVerified == false)
            {
                TempData["delete"] = "Please Verify Your Email";
                return RedirectToAction(nameof(Index));
            }
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                    return NotFound();

                if (user.Email == "shehabsameh987123@gmail.com")
                {
                    TempData["delete"] = "you can not delete this user";
                    return RedirectToAction(nameof(Index));
                }
                if (user.Id == signedUser.Id)
                {
                    TempData["delete"] = "you can not delete current user";
                    return RedirectToAction(nameof(Index));
                }

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
