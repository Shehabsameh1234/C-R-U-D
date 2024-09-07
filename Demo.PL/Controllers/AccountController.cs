using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace Demo.PL.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
			_userManager = userManager;
			_signInManager = signInManager;
		}

        #region signUp
        
        public IActionResult SignUp()
        {

            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    UserName = model.Email.Split("@")[0],
                    Email = model.Email,
                    IsAgree = model.IsAgree,
                    FName = model.FName,
                    LName = model.LName,
                };
              var result= await _userManager.CreateAsync(user,model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(LogIn));
                }
				foreach (var errors in result.Errors)
                {
					ModelState.AddModelError("Email", errors.Description);
					ModelState.AddModelError(string.Empty, errors.Description);

				}
			}
           
            return View(model);
        }
        #endregion

        #region SignIn
        
        public IActionResult LogIn()
        {
            if(_signInManager.IsSignedIn(User))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

                return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(LogInViewModel model)
        {
            if(ModelState.IsValid)
            {
                //get the user from data base(if exist)
                var user = await _userManager.FindByEmailAsync(model.Email);
                if(user is not  null) 
                {
                    var flag = await _userManager.CheckPasswordAsync(user, model.Password);
                    if(flag)
                    {
                       var result=await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                        if(result.Succeeded)
                        {
                                return RedirectToAction(nameof(HomeController.Index), "Home");
						}
                    }
                    
                }

				TempData["Message"] = "invalid email or password";
			}
            return View(model);
        }
		#endregion

		#region LogOut
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(LogIn));
        }
		#endregion

		#region forget password
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
		public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
		{
			if(ModelState.IsValid)
            {
                var user =await _userManager.FindByEmailAsync(model.Email);
                if(user != null)
                {
                    var token =await  _userManager.GeneratePasswordResetTokenAsync(user);
                    var resetPasswordUlr = Url.Action("ResetPassword", "Account", new { email = model.Email,token},Request.Scheme);
                    var email = new Email()
                    {
                        Title = "reset your password",
                        To = model.Email,
                        Body = resetPasswordUlr,
                    };
                    EmailSettings.SendEmail(email);
                    return RedirectToAction("CheckYourInbox");
                }
                ModelState.AddModelError(nameof(ForgetPasswordViewModel.Email), "email does not exist");
            }
			return View(model);
		}
        public IActionResult CheckYourInbox()
        {
            return View();
        }
        public IActionResult ResetPassword(string email,string token)
        {
            return View();
        }
        [HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if(ModelState.IsValid)
            {
				var user = await _userManager.FindByEmailAsync(model.Email);
                if(user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
					if (result.Succeeded)
					{
						return RedirectToAction(nameof(LogIn));
					}
					foreach (var errors in result.Errors)
					{
						ModelState.AddModelError("Email", errors.Description);
						ModelState.AddModelError(string.Empty, errors.Description);

					}
				}
			}
            return View(model);
		}
        #endregion

        #region email verifaction
        [HttpGet]
        public async Task<ActionResult> EmailVerifaction()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var user =await  _userManager.GetUserAsync(User);
                var email = new Email()
                {
                    Title = "Code For Verifaction",
                    To = user.Email,
                    Body = "1234",
                };
                EmailSettings.SendEmail(email);

            }

            return View();
        }
        [HttpPost]
        public async Task<ActionResult> EmailVerifaction(int code)
        {
            if(code ==1234)
            {
                return RedirectToAction("Index", "Home");
            }
            
           return View();
        }
        #endregion

    }
}
