using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Message.Event
{
    public class AnswerExamEvent
    {
        public int Id { get; set; }
        public int? ExamId { get; set; }
        public string? OptionsText { get; set; }
        public bool? CorrectAnswer { get; set; }
    }
}
