using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using _212711T_PracticalAssignment.ViewModels;
using Microsoft.AspNetCore.Identity;
using _212711T_PracticalAssignment.Model;
using System.Security.Claims;
using _212711T_PracticalAssignment.Recaptcha;

namespace _212711T_PracticalAssignment.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Login LModel { get; set; }

        private readonly SignInManager<ApplicationUser> signInManager;

        private readonly GoogleCaptchaService _captchaService;

        public LoginModel(SignInManager<ApplicationUser> signInManager, GoogleCaptchaService captchaService)
        {
            this.signInManager = signInManager;
            this._captchaService = captchaService;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // verify response token w google
            var captchaResult = await _captchaService.VerifyToken(LModel.Token);
            if (!captchaResult)
            {
                return Page();
            }

            if (ModelState.IsValid)
            {
                var identityResult = await signInManager.PasswordSignInAsync(LModel.Email, LModel.Password,
               LModel.RememberMe,lockoutOnFailure:true);
                if (identityResult.IsLockedOut)
                {
                    ModelState.AddModelError("LockoutError", "Try again 10 seconds later");
                    return Page();
                }
                else
                {
                    if (identityResult.Succeeded)
                    {
                        //Create the security context
                        var claims = new List<Claim> {
                        new Claim(ClaimTypes.Name, "c@c.com"),
                        new Claim(ClaimTypes.Email, "c@c.com"),
                        new Claim("Customer", "Membership")
                    };
                        var i = new ClaimsIdentity(claims, "MyCookieAuth");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(i);
                        await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);
                        return RedirectToPage("Index");
                    }
                    ModelState.AddModelError("LoginError", "Username or Password incorrect");
                }
                
            }
            return Page();
        }
    }
}
