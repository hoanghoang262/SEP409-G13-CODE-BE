namespace ModerationService.API.Common.ModelDTO
{
    public class CodeQuestionDTO
    {
        

        public int Id { get; set; }
        public string? Description { get; set; }
        public int? ChapterId { get; set; }

        public virtual ICollection<TestCaseDTO> TestCases { get; set; }
    }
}
