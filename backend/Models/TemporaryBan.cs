using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class TemporaryBan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public ulong MemberId { get; set; }
        public string Username { get; set; }
        public string Discriminator { get; set; }
        public DateTimeOffset ExpiresAt { get; set; }
    }
}