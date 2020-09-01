namespace backend.DTOs
{
    public class EditMemberDto
    {
        public string MemberId { get; set; }
        public string DisplayName { get; set; }
        public bool IsDeafened { get; set; }
        public bool IsMuted { get; set; }
    }
}