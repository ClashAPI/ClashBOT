using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class LeaveMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public bool IsEnabled { get; set; }
        public virtual DiscordChannel DiscordChannel { get; set; }
        public string Message { get; set; }
    }
}