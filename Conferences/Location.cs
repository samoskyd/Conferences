using System;
using System.Collections.Generic;

#nullable disable

namespace Conferences
{
    public partial class Location
    {
        public Location()
        {
            Conferences = new HashSet<Conference>();
        }

        public int LocationId { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public long Capacity { get; set; }

        public virtual ICollection<Conference> Conferences { get; set; }
    }
}
