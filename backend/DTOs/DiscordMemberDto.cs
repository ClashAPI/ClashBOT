namespace backend.DTOs
{
    public class DiscordMemberDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Avatar { get; set; }
        public string Discriminator { get; set; }
        public int PublicFlags { get; set; }
        public int Flags { get; set; }
        public string Email { get; set; }
        public bool Verified { get; set; }
        public string Locale { get; set; }
        public bool MfaEnabled { get; set; }
    }
}