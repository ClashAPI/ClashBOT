namespace backend.DTOs
{
    public class RoleActionDto
    {
        public ulong UserId { get; set; }
        public ulong RoleId { get; set; }
        public string? Reason { get; set; }
    }
}