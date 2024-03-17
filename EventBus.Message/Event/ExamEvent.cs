using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Message.Event
{
    public class ExamEvent
    {
        public int Id { get; set; }
        public string? ContentQuestion { get; set; }
        public int? Time { get; set; }
        public int? Score { get; set; }
        public bool? Status { get; set; }
        public int LastExamId { get; set; }
    }
}
