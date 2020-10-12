using System.Collections.Generic;

namespace backend.Models
{
    public class TwitchPlugin : Plugin
    {
        public virtual List<TwitchChannelSubscription> TwitchChannelSubscriptions { get; set; }
    }
}