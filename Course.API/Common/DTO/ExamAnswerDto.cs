namespace CourseService.API.Common.ModelDTO
{
    public class ExamAnswerDto
    {
        public int ExamId { get; set; }
        public List<int> SelectedAnswerIds { get; set; }
    }
}
