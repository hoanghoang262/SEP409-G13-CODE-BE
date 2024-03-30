namespace ModerationService.API.Common.DTO
{
    public class LastExamDTO
    {
        public int Id { get; set; }
        public int ChapterId { get; set; }
        public int? PercentageCompleted { get; set; }
        public string? Name { get; set; }
        public int? Time { get; set; }
        public virtual ICollection<QuestionExamDTO> QuestionExams { get; set; }
    }
    public class QuestionExamDTO
    {
        public int Id { get; set; }
        public string? ContentQuestion { get; set; }
        public int? Score { get; set; }
        public bool? Status { get; set; }
        public int LastExamId { get; set; }
        public virtual ICollection<AnswerExamDTO> AnswerExams { get; set; }

    }
    public class AnswerExamDTO
    {
        public int Id { get; set; }
        public int? ExamId { get; set; }
        public string? OptionsText { get; set; }
        public bool? CorrectAnswer { get; set; }
    }
}
