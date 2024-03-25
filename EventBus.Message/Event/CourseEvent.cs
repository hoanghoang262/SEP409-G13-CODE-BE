namespace EventBus.Message.Event
{
    public class CourseEvent
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Picture { get; set; }
        public string? Tag { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? Price { get; set; }
    }
}
