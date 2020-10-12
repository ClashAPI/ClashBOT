using System.Collections.Generic;
using backend.Models;

namespace backend.DTOs
{
    public class ScheduledMessagesPluginDto
    {
        public bool IsEnabled { get; set; }
        public List<ScheduledMessage> ScheduledMessages { get; set; }
    }
}