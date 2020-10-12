using System.Collections;
using System.Collections.Generic;
using backend.Models;

namespace backend.DTOs
{
    public class TwitchPluginDto
    {
        public bool IsEnabled { get; set; }
        public List<TwitchChannelSubscription> TwitchChannelSubscriptions { get; set; }
    }
}