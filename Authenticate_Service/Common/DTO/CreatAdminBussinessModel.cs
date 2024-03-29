using System.ComponentModel.DataAnnotations;

namespace AuthenticateService.API.Common.DTO
{
    public class CreatAdminBussinessModel
    {

        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }
       
        public string? Picture { get; set; }
    }
}
