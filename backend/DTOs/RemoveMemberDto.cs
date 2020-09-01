namespace backend.DTOs
{
    public class RemoveMemberDto
    {
        public string UserId { get; set; }
        public string Reason { get; set; }
        public bool NotifyUser { get; set; }
        public bool NotifyAnonymously { get; set; }
        public bool IncludeReason { get; set; }
    }
}