using System;
using System.Collections.Generic;

#nullable disable

namespace Conferences
{
    public partial class Conference
    {
        public Conference()
        {
            ConferencesAndParticipants = new HashSet<ConferencesAndParticipant>();
        }

        public int ConferenceId { get; set; }
        public string Title { get; set; }
        public string Aim { get; set; }
        public string Topic { get; set; }
        public int FormId { get; set; }
        public string RequirementsForWorks { get; set; }
        public string RequirementsForParticipants { get; set; }
        public decimal Price { get; set; }
        public DateTime DateAndTime { get; set; }
        public int OrganizerId { get; set; }
        public int LocationId { get; set; }

        public virtual Form Form { get; set; }
        public virtual Location Location { get; set; }
        public virtual Organizer Organizer { get; set; }
        public virtual ICollection<ConferencesAndParticipant> ConferencesAndParticipants { get; set; }
    }
}
