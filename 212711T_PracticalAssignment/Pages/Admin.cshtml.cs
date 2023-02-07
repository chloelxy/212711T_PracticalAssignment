using _212711T_PracticalAssignment.Model;
using _212711T_PracticalAssignment.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _212711T_PracticalAssignment.Pages
{
    [Authorize(Roles ="Admin")]
    public class AdminModel : PageModel
    {
        private UserManager<ApplicationUser> userManager { get; }
        private SignInManager<ApplicationUser> signInManager { get; }

        private readonly RoleManager<IdentityRole> roleManager;

        private IWebHostEnvironment _environment;

        [BindProperty]
        public IFormFile? Upload { get; set; }

        [BindProperty]
        public Admin AModel { get; set; }

        public AdminModel(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IWebHostEnvironment environment, RoleManager<IdentityRole>roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _environment = environment;
            this.roleManager = roleManager;
        }
        public void OnGet() 
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (ModelState.IsValid)
            {

                var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
                var protector = dataProtectionProvider.CreateProtector("MySecretKey");


                var user = new ApplicationUser()
                {
                    UserName = AModel.Email,
                    Email = AModel.Email,
                    FirstName = AModel.FirstName,
                    LastName = AModel.LastName,
                    Gender = AModel.Gender,
                    NRIC = protector.Protect(AModel.NRIC),
                    BirthDate = AModel.BirthDate,
                    WhoamI = AModel.WhoamI
                    /*  Resume = resumeUpload,*/

                };

                IdentityRole role = await roleManager.FindByIdAsync("Admin");
                if (role == null)
                {
                    IdentityResult result2 = await roleManager.CreateAsync(new IdentityRole("Admin"));
                    if (!result2.Succeeded)
                    {
                        ModelState.AddModelError("", "Create role admin failed");
                    }
                }
                var result = await userManager.CreateAsync(user, AModel.Password);
                if (result.Succeeded)
                {
                    /*await signInManager.SignInAsync(user, false);*/

                    result = await userManager.AddToRoleAsync(user, "Admin");
                    return Page();
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            return Page();
        }
    }
}
