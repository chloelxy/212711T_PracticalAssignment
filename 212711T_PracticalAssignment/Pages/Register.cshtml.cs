using _212711T_PracticalAssignment.Model;
using _212711T_PracticalAssignment.ViewModels;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting.Internal;

namespace _212711T_PracticalAssignment.Pages
{
    public class RegisterModel : PageModel
    {
        private UserManager<ApplicationUser> userManager { get; }
        private SignInManager<ApplicationUser> signInManager { get; }

        private readonly RoleManager<IdentityRole> roleManager;

        private IWebHostEnvironment _environment;

        [BindProperty]
        public IFormFile? Upload { get; set; }

        [BindProperty]
        public Register RModel { get; set; }

        public RegisterModel(UserManager<ApplicationUser> userManager,
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
               /* var resumeUpload = string.Empty;*/

                /*if (Upload != null)
                {
                    if (Upload.Length > 50 * 1024 * 1024)
                    {
                        ModelState.AddModelError("Upload", "File size cannot exceed 50MB.");
                        return Page();
                    }
                    var uploadsFolder = "uploads";
                    var resumeFile = Guid.NewGuid() + Path.GetExtension(RModel.Resume.FileName);
                    var resumePath = Path.Combine(_environment.ContentRootPath, "wwwroot", uploadsFolder, resumeFile);
                    using var fileStream = new FileStream(resumePath, FileMode.Create);
                    await Upload.CopyToAsync(fileStream);
                    resumeUpload = string.Format("/{0}/{1}", uploadsFolder, resumeFile);
                   *//* var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pdf", Upload.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        Upload.CopyTo(stream);
                    }
                    return RedirectToAction("ConvertToImage", new { fileName = Upload.FileName });*//*
                }*/
           /*     else
                {
                    ModelState.AddModelError("Upload", "File is not uploaded successfully");
                    return Page();
                }*/


                var dataProtectionProvider = DataProtectionProvider.Create("EncryptData");
                var protector = dataProtectionProvider.CreateProtector("MySecretKey");


                var user = new ApplicationUser()
                {
                    UserName = RModel.Email,
                    Email = RModel.Email,
                    FirstName = RModel.FirstName,
                    LastName = RModel.LastName,
                    Gender = RModel.Gender,
                    NRIC = protector.Protect(RModel.NRIC),
                    BirthDate = RModel.BirthDate,
                    WhoamI = RModel.WhoamI,
                 /*   Resume = resumeUpload,*/

                };

                IdentityRole role = await roleManager.FindByIdAsync("Member");
                if (role == null)
                {
                    IdentityResult result2 = await roleManager.CreateAsync(new IdentityRole("Member"));
                    if (!result2.Succeeded)
                    {
                        ModelState.AddModelError("", "Create role member failed");
                    }
                }

                var result = await userManager.CreateAsync(user, RModel.Password);
                if (result.Succeeded)
                {
                    /*await signInManager.SignInAsync(user, false);*/
                    result = await userManager.AddToRoleAsync(user, "Member");
                    return RedirectToPage("/Login");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            return Page();
        }

        /*private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }*/

        /*public IActionResult ConvertPDFToImage(string fileName)
        {
            var pdfFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pdf", fileName);
            var image = ExtractImagesFromPDF(pdfFile);
            // Save the image to wwwroot/images directory
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName + ".jpg");
            image.Save(imagePath, ImageFormat.Jpeg);
            // Convert the image to byte array
            byte[] imageData;
            using (var memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, ImageFormat.Jpeg);
                imageData = memoryStream.ToArray();
            }
            // Store the image in SQL database
            using (var connection = new SqlConnection("<your connection string>"))
            {
                connection.Open();
                var command = new SqlCommand("INSERT INTO Images (FileName, ImageData) VALUES (@FileName, @ImageData)", connection);
                command.Parameters.AddWithValue("@FileName", fileName);
                command.Parameters.AddWithValue("@ImageData", imageData);
                command.ExecuteNonQuery();
            }
            return Content("PDF converted to image and stored in database");
        }*/

    }
}
