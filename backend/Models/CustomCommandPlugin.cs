using System.Collections.Generic;

namespace backend.Models
{
    public class CustomCommandPlugin : Plugin
    {
        public virtual IList<Command> Commands { get; set; }
        public virtual IList<AdvancedCommand> AdvancedCommands { get; set; }
    }
}