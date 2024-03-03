using CourseService.API.Common.Mapping;
using ModerationService.API.Models;

namespace ModerationService.API.Common.ModelDTO
{
    public class LessonDTO : IMapFrom<Lesson>
    {
     

        public int Id { get; set; }
        public string? Title { get; set; }
        public string? VideoUrl { get; set; }
        public int? ChapterId { get; set; }
        public string? Description { get; set; }
        public long? Duration { get; set; }
        public string? ContentLesson { get; set; }


        public virtual ICollection<TheoryQuestionDTO> Questions { get; set; }

    }
}
