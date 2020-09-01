using backend.Helpers;
using System.Collections.Generic;

namespace backend.Models
{
    public class AutoModPlugin : Plugin
    {
        public virtual BadWordsSettings BadWordsSettings { get; set; }
        public virtual RepeatedTextSettings RepeatedTextSettings { get; set; }
        public virtual ServerInvitesSettings ServerInvitesSettings { get; set; }
        public virtual ExternalLinksSettings ExternalLinksSettings { get; set; }
        public virtual ExcessiveCapsSettings ExcessiveCapsSettings { get; set; }
        public virtual ExcessiveEmojisSettings ExcessiveEmojisSettings { get; set; }
        public virtual ExcessiveSpoilersSettings ExcessiveSpoilersSettings { get; set; }
        public virtual ExcessiveMentionsSettings ExcessiveMentionsSettings { get; set; }
        public virtual ZalgoSettings ZalgoSettings { get; set; }
        public virtual IList<AutomatedAction> AutomatedActions { get; set; }
        public virtual IList<ModeratorCommand> ModeratorCommands { get; set; }
        public virtual List<ModeratorRole> ModeratorRoles { get; set; }
        public bool IgnoreBots { get; set; }
    }
}