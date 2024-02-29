using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Message.Event
{
    public class AnswerOptionsEvent
    {
        public int Id { get; set; }
        public int? QuestionId { get; set; }
        public string? OptionsText { get; set; }
        public bool? CorrectAnswer { get; set; }
    }
}
