namespace backend.DTOs
{
    public class CustomCommandDto
    {
        public string CommandCall { get; set; }
        public string Description { get; set; }
        public string Response { get; set; }
        public char Prefix { get; set; }
    }
}