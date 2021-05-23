using System;
using System.Collections.Generic;

#nullable disable

namespace Conferences
{
    public partial class Form
    {
        public Form()
        {
            Conferences = new HashSet<Conference>();
        }

        public int FormId { get; set; }
        public string FullName { get; set; }
        public string AvailableAudienceSize { get; set; }

        public virtual ICollection<Conference> Conferences { get; set; }
    }
}
