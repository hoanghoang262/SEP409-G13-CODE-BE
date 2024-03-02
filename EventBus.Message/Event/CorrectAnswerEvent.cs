using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Message.Event
{
    public class CorrectAnswerEvent
    {
        public int Id { get; set; }
        public int? QuestionId { get; set; }
        public int? OptionsId { get; set; }
    }
}
