using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Command
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string CommandCall { get; set; }
        public string Description { get; set; }
        public string Response { get; set; }
        public char Prefix { get; set; }
        public bool IsEnabled { get; set; }
    }
}