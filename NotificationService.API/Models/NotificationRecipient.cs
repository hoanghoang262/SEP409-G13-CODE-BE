using System;
using System.Collections.Generic;

namespace NotificationService.API.Models
{
    public partial class NotificationRecipient
    {
        public int Id { get; set; }
        public int? RecipientId { get; set; }
        public int? NotificationId { get; set; }

        public virtual Notification? Notification { get; set; }
    }
}
