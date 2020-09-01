using System;

namespace backend.Models
{
    public class LogEntry
    {
        public Guid Id { get; set; }
        public string ActionName { get; set; }
        public ActionType ActionType { get; set; }
        public virtual User Initiator { get; set; }
        public DateTime Date { get; set; }
        public string? GuildId { get; set; }

        public LogEntry()
        {
            Id = Guid.NewGuid();
            Date = DateTime.Now;
        }
    }
}