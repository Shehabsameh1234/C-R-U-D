using Demo.DAL.Models;
using Demo.PL.ViewModels;
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
	public class RoleController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserManager<ApplicationUser> _UserManager { get; }

        public RoleController(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _UserManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ApplicationRole applicationRole)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(applicationRole);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                foreach (var errors in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, errors.Description);
                }
            }
            return View(applicationRole);
        }

        public async Task<IActionResult> Details(string id, string ViewName = "Details")
        {
            if (id == null)
            {
                return BadRequest();
            }
            var Role = await _roleManager.FindByIdAsync(id);
            if (Role == null)
            {
                return NotFound();
            }
            return View(ViewName, Role);
        }
        public async Task<IActionResult> Edit(string id)
        {
            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, ApplicationRole applicationRole)
        {

            if (id != applicationRole.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _roleManager.FindByIdAsync(id);
                    user.Name = applicationRole.Name;
                    user.Name = applicationRole.Name.ToUpper();
                    var result = await _roleManager.UpdateAsync(user);
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
            return View(applicationRole);

        }
        public async Task<IActionResult> Delete([FromRoute] string id)
        {

            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null)
                    return NotFound();
                if (role.Name == "Admin")
                {
                    TempData["delete"] = "you can not delete this role";
                    return RedirectToAction(nameof(Index));
                }

                var result = await _roleManager.DeleteAsync(role);
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
        
        public async Task<IActionResult> AddOrRemoveUsers1(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            ViewBag.roleId = roleId;
            if (role == null)
                NotFound();
            var UsersInRole = new List<UserInRoleViewmodel>();
            var users = await _UserManager.Users.ToArrayAsync();
            foreach (var user in users)
            {
                var UserInRole = new UserInRoleViewmodel
                {
                    UserID = user.Id,
                    UserName = user.UserName,
                };
                if (await _UserManager.IsInRoleAsync(user, role.Name))
                    UserInRole.IsSelected = true;
                else UserInRole.IsSelected = false;
                UsersInRole.Add(UserInRole);
            }
            return View(UsersInRole);
        }
        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId, List<UserInRoleViewmodel> users)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            
            if (role == null)
                NotFound();
            if (ModelState.IsValid)
            {
                foreach (var user in users)
                {
                    var appUser = await _UserManager.FindByIdAsync(user.UserID);
                    if (appUser != null)
                    {
                      
                        if (user.IsSelected && !await _UserManager.IsInRoleAsync(appUser, role.Name))
                            await _UserManager.AddToRoleAsync(appUser, role.Name);
                        else if (!user.IsSelected && await _UserManager.IsInRoleAsync(appUser, role.Name))
                            await _UserManager.RemoveFromRoleAsync(appUser, role.Name);
                    }

                }
                return RedirectToAction("LogOut","Account");
            }
            return View(users);

        }
    }
}
