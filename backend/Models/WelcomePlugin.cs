using System.Collections.Generic;

namespace backend.Models
{
    public class WelcomePlugin : Plugin
    {
        public virtual WelcomeMessage WelcomeMessage { get; set; }
        public virtual PrivateWelcomeMessage PrivateWelcomeMessage { get; set; }
        public virtual IList<DiscordRole> RolesToGive { get; set; }
        public virtual LeaveMessage LeaveMessage { get; set; }
    }
}