using System;
using System.Collections.Generic;

#nullable disable

namespace Conferences
{
    public partial class Work
    {
        public Work()
        {
            WorksAndParticipants = new HashSet<WorksAndParticipant>();
        }

        public int WorkId { get; set; }
        public int ParticipantId { get; set; }
        public string Title { get; set; }
        public string Topic { get; set; }
        public DateTime PublicationDate { get; set; }
        public int ConferenceId { get; set; }

        public virtual ICollection<WorksAndParticipant> WorksAndParticipants { get; set; }
    }
}
