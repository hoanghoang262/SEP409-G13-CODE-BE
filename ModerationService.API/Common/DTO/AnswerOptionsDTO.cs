using CourseService.API.Common.Mapping;
using ModerationService.API.Models;

namespace ModerationService.API.Common.ModelDTO
{
    public class AnswerOptionsDTO : IMapFrom<AnswerOption>
    {
        public int Id { get; set; }
        public int? QuestionId { get; set; }
        public string? OptionsText { get; set; }

        public bool? CorrectAnswer { get; set; }




    }
}
