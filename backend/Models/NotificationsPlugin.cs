using System.Collections.Generic;

namespace backend.Models
{
    public class NotificationsPlugin : Plugin
    {
        public virtual List<TwitchNotification> TwitchNotifications { get; set; }
    }
}