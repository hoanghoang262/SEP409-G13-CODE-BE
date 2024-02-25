using CourseService.API.Common.Mapping;
using ModerationService.API.Models;

namespace ModerationService.API.Common.ModelDTO
{
    public class ChapterDTO : IMapFrom<Chapter>
    {


        public int Id { get; set; }
        public string? Name { get; set; }
        public int? CourseId { get; set; }
        public decimal? Part { get; set; }
        public bool? IsNew { get; set; }

        public virtual ICollection<CodeQuestionDTO> CodeQuestions { get; set; }
        public virtual ICollection<LessonDTO> Lessons { get; set; }

    }
}
