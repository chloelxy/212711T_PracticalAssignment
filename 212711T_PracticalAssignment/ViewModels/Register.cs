using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace _212711T_PracticalAssignment.ViewModels
{
    public class Register
    {


        [Required(ErrorMessage = "Email is required.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required."), RegularExpression(@"^(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?=.*[^a-za-z0-9])(?!.*\s).{12,}", ErrorMessage ="Password must have at least 1 uppercase letter, lowercase letter, digit, special character and min. 12 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage ="Password is required."), RegularExpression(@"^(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?=.*[^a-za-z0-9])(?!.*\s).{12,}", ErrorMessage = "Password must have at least 1 uppercase letter, lowercase letter, digit, special character and min. 12 characters")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation password does not match")]
        public string ConfirmPassword { get; set; }

        [Required, RegularExpression(@"^[A-Za-z]+$", ErrorMessage ="First name can only consist of letters")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required, RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Last name can only consist of letters")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required, MaxLength(1)]
        public string Gender { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Text)]
        public string NRIC { get; set; } 

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime BirthDate { get; set; } = new DateTime(DateTime.Now.Year -
        18, 1, 1);

        [Required(ErrorMessage = "Who am I is required.")]
        [DataType(DataType.Text)]
        [Display(Name = "Who Am I")]
        public string WhoamI { get; set; }

        [DataType(DataType.Upload)]
        [MaxLength(50)]
        public string? Resume { get; set; }

    }
}
