using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class AutomatedAction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public ModerationAction ModerationAction { get; set; }
        public int InfractionsLimit { get; set; }
        public int TimeLimitInSeconds { get; set; }
    }
}