using System;
using System.Collections.Generic;

#nullable disable

namespace Conferences
{
    public partial class Participant
    {
        public Participant()
        {
            ConferencesAndParticipants = new HashSet<ConferencesAndParticipant>();
            WorksAndParticipants = new HashSet<WorksAndParticipant>();
        }

        public int ParticipantId { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string Occupation { get; set; }
        public string Institution { get; set; }

        public virtual ICollection<ConferencesAndParticipant> ConferencesAndParticipants { get; set; }
        public virtual ICollection<WorksAndParticipant> WorksAndParticipants { get; set; }
    }
}
