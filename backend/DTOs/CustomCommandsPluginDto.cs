using backend.Models;
using System.Collections.Generic;

namespace backend.DTOs
{
    public class CustomCommandsPluginDto
    {
        public bool IsEnabled { get; set; }
        public IList<Command> Commands { get; set; }
        public IList<AdvancedCommand> AdvancedCommands { get; set; }
    }
}