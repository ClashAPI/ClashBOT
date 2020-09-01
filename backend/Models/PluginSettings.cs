using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class PluginSettings
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public virtual IList<DiscordChannel> IgnoredChannels { get; set; }
        public virtual IList<DiscordRole> AllowedRoles { get; set; }
        public ModerationAction ModerationAction { get; set; }
        public virtual Guild Guild { get; set; }
    }
}