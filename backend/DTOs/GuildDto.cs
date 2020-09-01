using DSharpPlus.Entities;
using System.Collections.Generic;

namespace backend.DTOs
{
    public class GuildDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
        public string Icon { get; set; }
        public bool Owner { get; set; }
        public string Permissions { get; set; }
        public IList<string> Features { get; set; }
        public VerificationLevel VerificationLevel { get; set; }
        public ExplicitContentFilter ExplicitContentFilter { get; set; }
        public DefaultMessageNotifications DefaultMessageNotifications { get; set; }
        public bool IsAvailable { get; set; }
        public List<ManagerDto> Managers { get; set; }
    }
}
