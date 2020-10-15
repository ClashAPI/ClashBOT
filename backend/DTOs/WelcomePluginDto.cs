using System.Collections.Generic;
using backend.Models;

namespace backend.DTOs
{
    public class WelcomePluginDto
    {
        public bool IsEnabled { get; set; }
        public WelcomeMessage WelcomeMessage { get; set; }
        public PrivateWelcomeMessage PrivateWelcomeMessage { get; set; }
        public IList<DiscordRole> RolesToGive { get; set; }
        public LeaveMessage LeaveMessage { get; set; }
    }
}