using CourseService.API.Common.Mapping;
using ModerationService.API.Models;

namespace ModerationService.API.Common.ModelDTO
{
    public class TheoryQuestionDTO : IMapFrom<TheoryQuestion>
    {

        public int Id { get; set; }
        public int? VideoId { get; set; }
        public string? ContentQuestion { get; set; }
        public long? Time { get; set; }
        public string? TimeQuestion { get; set; }


        public virtual ICollection<AnswerOptionsDTO> AnswerOptions { get; set; }
       

    }
}
