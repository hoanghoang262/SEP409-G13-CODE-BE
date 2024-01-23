
using CourseService.API.Common.Mapping;

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

        public virtual ICollection<ChapterDTO> Chapters { get; set; }
    }
    public class ChapterDTO: IMapFrom<Chapter> {
       

        public int Id { get; set; }
        public string? Name { get; set; }
        public int? CourseId { get; set; }
        public decimal? Part { get; set; }
        public bool? IsNew { get; set; }

        
        public virtual ICollection<LessonDTO> Lessons { get; set; }

    }
    public class LessonDTO: IMapFrom<Lesson> 
    {
        public LessonDTO()
        {
            Comments = new HashSet<CommentDTO>();
            Questions = new HashSet<QuestionDTO>();
        }

        public int Id { get; set; }
        public string? Title { get; set; }
        public string? VideoUrl { get; set; }
        public IFormFile? VideoByte { get; set; }
        public int? ChapterId { get; set; }
        public string? Description { get; set; }
        public long? Duration { get; set; }

       
        public virtual ICollection<CommentDTO> Comments { get; set; }
        public virtual ICollection<QuestionDTO> Questions { get; set; }

    }
    public class QuestionDTO: IMapFrom<Question> {
        public int Id { get; set; }
        public int? VideoId { get; set; }
        public string? ContentQuestion { get; set; }
        public string? AnswerA { get; set; }
        public string? AnswerB { get; set; }
        public string? AnswerC { get; set; }
        public string? AnswerD { get; set; }
        public string? CorrectAnswer { get; set; }
        public long? Time { get; set; }

        
    }
    public class CommentDTO: IMapFrom<Comment>
    {
        public int Id { get; set; }
        public int? LessonId { get; set; }
        public string? CommentContent { get; set; }
        public string? UserId { get; set; }

        public virtual LessonDTO? Lesson { get; set; }
    }
}
