using System.ComponentModel.DataAnnotations;

namespace AuthenticateService.API.Common.DTO
{
    public class SignUpModel
    {
        public string UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
    }
}
