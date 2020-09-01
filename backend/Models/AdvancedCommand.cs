using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class AdvancedCommand
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string CommandCall { get; set; }
        public string Description { get; set; }
        public char Prefix { get; set; }
        public virtual IList<CommandAction> Actions { get; set; }
        public bool IsEnabled { get; set; }
    }
}