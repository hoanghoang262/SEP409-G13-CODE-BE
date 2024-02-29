using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Message.Event
{
    public class TheoryQuestionEvent
    {
        public int Id { get; set; }
        public int? VideoId { get; set; }
        public long? Time { get; set; }
        public string? ContentQuestion { get; set; }
        public long? TimeQuestion { get; set; }

    }
}
