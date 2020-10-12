using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class TwitchChannelSubscription
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string StreamerId { get; set; }
        public string ChannelId { get; set; }
        public virtual Guild Guild { get; set; }
    }
}