using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Message.Event
{
    public class LessonEvent
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? VideoUrl { get; set; }
        public int? ChapterId { get; set; }
        public string? Description { get; set; }
        public bool? IsCompleted { get; set; }
        public long? Duration { get; set; }
        public string? ContentLesson { get; set; }
        public string? CodeForm { get; set; }
    }
}
