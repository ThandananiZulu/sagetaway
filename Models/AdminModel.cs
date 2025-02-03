using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sagetaway.Models
{
    public class Admins
    {
        
       
        public int Id { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        // This is for the form input, not to be stored in the database
        [NotMapped]
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string PasswordInput { get; set; }

        // This will be sted in the database and must be populated by hashing the password
        
        public string PasswordHash { get; set; }  // No validation needed here

    }
}
