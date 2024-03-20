using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Message.Event
{
    public class UserEnrollEvent
    {
        public int? UserId { get; set; } 

        public int? CourseId { get; set; }   
    }
}
