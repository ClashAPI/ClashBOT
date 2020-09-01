using backend.Models;
using System.Collections.Generic;

namespace backend.DTOs
{
    public class CustomAdvancedCommandDto
    {
        public string CommandCall { get; set; }
        public string Description { get; set; }
        public char Prefix { get; set; }
        public IList<CommandAction> Actions { get; set; }
        public bool IsEnabled { get; set; }
    }
}