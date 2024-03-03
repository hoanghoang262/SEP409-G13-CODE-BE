using System.ComponentModel.DataAnnotations;

namespace AuthenticateService.API.Common.DTO
{
    public class LoginModels
    {
        [Required(ErrorMessage = "UserName is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
