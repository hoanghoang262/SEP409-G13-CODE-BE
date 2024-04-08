using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Message.Event
{
    public class PracticeQuestionEvent
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public int? ChapterId { get; set; }
        public string? CodeForm { get; set; }
        public string? TestCaseJava { get; set; }
        public string? TestCaseC { get; set; }
        public string? TestCaseCplus { get; set; }
        public string? Title { get; set; }
    }
}
