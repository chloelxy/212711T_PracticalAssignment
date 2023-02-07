using _212711T_PracticalAssignment.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using _212711T_PracticalAssignment.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.DataProtection;
using System.Runtime.CompilerServices;

namespace _212711T_PracticalAssignment.Pages
{

    [Authorize]

    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly AuthDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        [BindProperty]
        public ApplicationUser member { get; set; }

        public IndexModel(ILogger<IndexModel> logger, AuthDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public const string SessionKeyName = "_Name";
        public const string SessionKeyEmail = "_Email";

        public IActionResult OnGet()
        {
          /*  string sessionName = Request.Cookies["AceJobAgency"];
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyName)))
            {
                HttpContext.Session.Remove("AceJobAgency");
                _signInManager.SignOutAsync();
                HttpContext.Response.Redirect("Login");
                
                

            }*/
            /*var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.AspNetUsers.Where(u => u.Id == userId).FirstOrDefaultAsync();*/
            /*var name = HttpContext.Session.GetString(SessionKeyName);
            var email = HttpContext.Session.GetString(SessionKeyEmail);

            _logger.LogInformation("Session Name: {Name}", name);
            _logger.LogInformation("Session Email: {Email}", email);*/

            // display details

            string id = _userManager.GetUserId(User);
            ApplicationUser? currentUser = _context.Users.FirstOrDefault(u => u.Id.Equals(id));

            var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
            var protector = dataProtectionProvider.CreateProtector("MySecretKey");

            member = currentUser;
            member.NRIC = protector.Unprotect(currentUser.NRIC);

            return Page();
        }

        /*public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan. FromMinutes(30);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }*/
    }
}