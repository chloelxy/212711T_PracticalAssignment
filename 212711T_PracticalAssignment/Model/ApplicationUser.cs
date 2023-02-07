using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace _212711T_PracticalAssignment.Model
{
	public class ApplicationUser : IdentityUser
	{
        public string FirstName { get; set; }

     
        public string LastName { get; set; }

        public string Gender { get; set; }

        
        public string NRIC { get; set; } 


        public DateTime BirthDate { get; set; } 

      
        public string WhoamI { get; set; }

      
        public string? Resume { get; set; }
       
    }
}
