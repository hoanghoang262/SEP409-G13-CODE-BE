using System.ComponentModel.DataAnnotations;

namespace Account.API.Model
{
    public class LoginModels
    {
        [Required(ErrorMessage = "UserName is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
