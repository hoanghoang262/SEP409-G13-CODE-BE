using CourseService.API.Common.Mapping;
using ModerationService.API.Models;

namespace ModerationService.API.Common.ModelDTO
{
    public class PracticeQuestionDTO : IMapFrom<PracticeQuestion>
    {


        public int Id { get; set; }
        public string? Description { get; set; }
        public int? ChapterId { get; set; }
        public string? CodeForm { get; set; }
        public string? TestCaseJava { get; set; }
        public virtual ICollection<TestCaseDTO> TestCases { get; set; }
     
    }
}
