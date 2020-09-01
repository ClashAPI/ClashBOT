using backend.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Helpers
{
    public class ModeratorRole
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string RoleId { get; set; }
        public virtual AutoModPlugin AutoModPlugin { get; set; }
    }
}