using System;

namespace backend.DTOs
{
    public class TemporaryBanDto
    {
        public ulong MemberId { get; set; }
        public string Username { get; set; }
        public string Discriminator { get; set; }
        public DateTimeOffset ExpiresAt { get; set; }
    }
}