using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class User : IdentityUser<Guid>
    {
        public User()
        {
            CreatedAt = DateTime.Now;
            SeenPatchNotes = new List<SeenPatchNote>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string UserId { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public string Discriminator { get; set; }
        public bool IsSuperuser { get; set; }
        // [Encrypted]
        // public string AccessToken { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual IList<SeenPatchNote> SeenPatchNotes { get; set; }
    }
}