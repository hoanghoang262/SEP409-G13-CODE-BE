using System.ComponentModel.DataAnnotations;

namespace Account.API.Model
{
    public class LoginModelsDTO
    {
        [Required(ErrorMessage = "UserName is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
