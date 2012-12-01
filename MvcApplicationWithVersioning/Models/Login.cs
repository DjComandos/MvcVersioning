using System.ComponentModel.DataAnnotations;

namespace MvcApplicationWithVersioning.Models
{
    public class Login
    {
        [DataType(DataType.EmailAddress)]
        [Display(Description = "Email")] // yep, no resources...
        [Required(ErrorMessage = "Please enter your Email")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Description = "Password")]
        [Required(ErrorMessage = "Please enter your password")]
        public string Password { get; set; }
    }
}