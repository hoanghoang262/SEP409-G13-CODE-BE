using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Message.Event
{
    public class NotificationEvent
    {
        public int NotificationsId { get; set; }
        public int? RecipientId { get; set; }
        public string? NotificationContent { get; set; }
        public int Course_Id { get; set; }
        public DateTime SendDate { get; set; }
        public bool IsSeen { get; set; }

    }
}
