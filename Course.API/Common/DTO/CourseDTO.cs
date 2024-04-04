using CourseService.API.Common.Mapping;
using CourseService.API.Models;

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
        public DateTime? CreatedAt { get; set; }
        public string? Enrolled { get; set; }

        public virtual ICollection<ChapterDTO> Chapters { get; set; }
    }
    public class ChapterDTO: IMapFrom<Chapter> {
       

        public int Id { get; set; }
        public string? Name { get; set; }
        public int? CourseId { get; set; }
        public decimal? Part { get; set; }
        public bool? IsNew { get; set; }

        
        public virtual ICollection<LessonDTO> Lessons { get; set; }
        public virtual ICollection<PracticeQuestionDTO> PracticeQuestions { get; set; }

    }
    public class LessonDTO: IMapFrom<Lesson> 
    {
       

        public int Id { get; set; }
        public string? Title { get; set; }
        public string? VideoUrl { get; set; }
        public int? ChapterId { get; set; }
        public string? Description { get; set; }
        public long? Duration { get; set; }
        public string? ContentLesson { get; set; }
        public string? ChapterName { get; set; }
        public string? CourseName { get; set; }

        public bool? IsCompleted { get; set; }
        public string? CodeForm { get; set; }



        public virtual ICollection<TheoryQuestionDTO> TheoryQuestions { get; set; }

    }
    public class TheoryQuestionDTO : IMapFrom<TheoryQuestion> {
        public int Id { get; set; }
        public int? VideoId { get; set; }
        public long? Time { get; set; }
        public string? ContentQuestion { get; set; }
        public long? TimeQuestion { get; set; }
        public virtual ICollection<AnswerOptionDTO> AnswerOptions { get; set; }

    }
    public class UserProfileDto
    {

        public List<CourseCompletionDto> EnrolledCourses { get; set; }
    }

    public class CourseCompletionDto
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CoursePicture { get; set; }
        public double CompletionPercentage { get; set; }
    }
    public class PracticeQuestionDTO :IMapFrom<PracticeQuestion>
    {

        public int Id { get; set; }
        public string? Description { get; set; }
        public int? ChapterId { get; set; }
        public string? CodeForm { get; set; }
        public string? TestCaseJava { get; set; }
        public string? ChapterName  { get; set; }
        public string? CourseName  { get; set; }

        public virtual ICollection<TestCase> TestCases { get; set; }
        public virtual ICollection<UserAnswerCode> UserAnswerCodes { get; set; }

    }
    public class AnswerOptionDTO 
    {
        public int Id { get; set; }
        public int? QuestionId { get; set; }
        public string? OptionsText { get; set; }
        public bool? CorrectAnswer { get; set; }

    }
}
