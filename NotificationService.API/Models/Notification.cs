using System;
using System.Collections.Generic;

namespace NotificationService.API.Models
{
    public partial class Notification
    {
        public Notification()
        {
            NotificationRecipients = new HashSet<NotificationRecipient>();
        }

        public int Id { get; set; }
        public string? NotificationContent { get; set; }
        public DateTime? SendDate { get; set; }
        public bool? IsSeen { get; set; }
        public int? RecipientId { get; set; }
        public int? CourseId { get; set; }
        public int? PostId { get; set; }

        public virtual ICollection<NotificationRecipient> NotificationRecipients { get; set; }
    }
}
