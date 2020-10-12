using System.Collections.Generic;

namespace backend.Models
{
    public class ScheduledMessagesPlugin : Plugin
    {
        public virtual List<ScheduledMessage> ScheduledMessages { get; set; }
    }
}