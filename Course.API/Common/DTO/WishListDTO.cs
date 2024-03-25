namespace CourseService.API.Common.DTO
{
    public class WishListDTO
    {
        public int Id { get; set; }
        public int? CourseId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Picture { get; set; }
        public string? Tag { get; set; }
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
