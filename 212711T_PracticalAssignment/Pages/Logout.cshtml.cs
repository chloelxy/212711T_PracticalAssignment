using _212711T_PracticalAssignment.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Org.BouncyCastle.Asn1.Ocsp;

namespace _212711T_PracticalAssignment.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        public LogoutModel(SignInManager<ApplicationUser> signInManager)
        {
            this.signInManager = signInManager;
        }

        public void OnGet()
        {
        }

        
        public async Task<IActionResult> OnPostLogoutAsync()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("AceJobAgency")))
            {
                HttpContext.Session.Remove("AceJobAgency");

                if (string.IsNullOrEmpty(HttpContext.Session.GetString("MyCookieAuth")))
                { 
                    HttpContext.Session.Remove("MyCookieAuth");
                    signInManager.SignOutAsync();
                    HttpContext.Response.Redirect("Login");

                }

            }

            await signInManager.SignOutAsync();
            return RedirectToPage("Login");
        }
        public async Task<IActionResult> OnPostDontLogoutAsync()
        {
            return RedirectToPage("/Index");
        }
    }
}
