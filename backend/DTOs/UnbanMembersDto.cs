using System.Collections.Generic;

namespace backend.DTOs
{
    public class UnbanMembersDto
    {
        public IList<UnbanDto> Members { get; set; }
    }
}