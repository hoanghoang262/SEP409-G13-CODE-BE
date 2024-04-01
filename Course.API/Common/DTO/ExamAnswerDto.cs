namespace CourseService.API.Common.ModelDTO
{
    public class ExamAnswerDto
    {
        public int Id { get; set; }
        public List<int> SelectedAnswerIds { get; set; }
    }
}
