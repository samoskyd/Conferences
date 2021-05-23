using System;
using System.Collections.Generic;

#nullable disable

namespace Conferences
{
    public partial class ConferencesAndParticipant
    {
        public int ConferenceAndParticipantId { get; set; }
        public int ParticipantId { get; set; }
        public int ConferenceId { get; set; }

        public virtual Conference Conference { get; set; }
        public virtual Participant Participant { get; set; }
    }
}
