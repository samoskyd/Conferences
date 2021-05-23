using System;
using System.Collections.Generic;

#nullable disable

namespace Conferences
{
    public partial class WorksAndParticipant
    {
        public int WorkAndParticipantId { get; set; }
        public int WorkId { get; set; }
        public int ParticipantId { get; set; }

        public virtual Participant Participant { get; set; }
        public virtual Work Work { get; set; }
    }
}
