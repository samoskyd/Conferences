using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Conferences
{
    public partial class ConferencesAndParticipant
    {
        public int ConferenceAndParticipantId { get; set; }
        [Required(ErrorMessage = "це поле не можна залишати порожнім")]
        [Display(Name = "Учасник")]
        public int ParticipantId { get; set; }
        [Required(ErrorMessage = "заповніть поле, ок?")]
        [Display(Name = "Конференція")]
        public int ConferenceId { get; set; }

        public virtual Conference Conference { get; set; }
        public virtual Participant Participant { get; set; }
    }
}
