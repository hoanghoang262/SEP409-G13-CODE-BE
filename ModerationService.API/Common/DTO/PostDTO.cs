namespace ModerationService.API.Common.DTO
{
    public class PostDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? PostContent { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? LastUpdate { get; set; }
        public string UserPicture {  get; set; }    
        public string UserName { get; set; }    


    }
}
