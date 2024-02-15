using CourseService.API.Common.Mapping;
using ModerationService.API.Common.ModelDTO;
using ModerationService.API.Models;

namespace CourseService.API.Common.ModelDTO
{
    public class CourseDTO : IMapFrom<Course>
    {
        public CourseDTO()
        {
            Chapters = new HashSet<ChapterDTO>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Picture { get; set; }
        public string? Tag { get; set; }
        public int? UserId { get; set; }
        public string? UserName { get; set; }

        public virtual ICollection<ChapterDTO> Chapters { get; set; }
    }
   
 
   
}
