using System.Collections.Generic;

namespace backend.Models
{
    public class ExternalLinksSettings : PluginSettings
    {
        public virtual IList<Website> AllowedWebsites { get; set; }
    }
}