using System;
using System.Collections.Generic;

#nullable disable

namespace Conferences
{
    public partial class Organizer
    {
        public Organizer()
        {
            Conferences = new HashSet<Conference>();
        }

        public int OrganizerId { get; set; }
        public string FullName { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string Occupation { get; set; }

        public virtual ICollection<Conference> Conferences { get; set; }
    }
}
