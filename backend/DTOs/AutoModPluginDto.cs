using backend.Helpers;
using backend.Models;
using System.Collections.Generic;

namespace backend.DTOs
{
    public class AutoModPluginDto
    {
        public BadWordsSettings BadWordsSettings { get; set; }
        public RepeatedTextSettings RepeatedTextSettings { get; set; }
        public ServerInvitesSettings ServerInvitesSettings { get; set; }
        public ExternalLinksSettings ExternalLinksSettings { get; set; }
        public ExcessiveCapsSettings ExcessiveCapsSettings { get; set; }
        public ExcessiveEmojisSettings ExcessiveEmojisSettings { get; set; }
        public ExcessiveSpoilersSettings ExcessiveSpoilersSettings { get; set; }
        public ExcessiveMentionsSettings ExcessiveMentionsSettings { get; set; }
        public ZalgoSettings ZalgoSettings { get; set; }
        public IList<AutomatedAction> AutomatedActions { get; set; }
        public IList<ModeratorCommand> ModeratorCommands { get; set; }
        public List<ModeratorRole> ModeratorRoles { get; set; }
        public bool IgnoreBots { get; set; }

        public bool IsEnabled { get; set; }
        /*
        public bool IsEnabled { get; set; }
        public virtual IList<string> BlacklistedWords { get; set; }
        public virtual IList<ModeratorCommand> ModeratorCommands { get; set; }
        public virtual List<ModeratorRole> ModeratorRoles { get; set; }
        */
    }
}