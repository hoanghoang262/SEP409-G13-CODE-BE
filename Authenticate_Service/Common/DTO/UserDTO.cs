namespace AuthenticateService.API.Common.DTO
{
    public class UserDto
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? FacebookLink { get; set; }
        public string? ProfilePict { get; set; }
        public bool? Status { get; set; }
        public int? RoleId { get; set; }
        public string? RoleName { get; set; }
    }
}
