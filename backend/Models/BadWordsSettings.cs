using System.Collections.Generic;

namespace backend.Models
{
    public class BadWordsSettings : PluginSettings
    {
        public virtual IList<BadWord> BadWords { get; set; }
    }
}