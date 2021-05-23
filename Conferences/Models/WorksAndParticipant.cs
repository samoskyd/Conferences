using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Conferences
{
    public partial class WorksAndParticipant
    {
        public int WorkAndParticipantId { get; set; }

        [Required(ErrorMessage = "це поле не можна залишати порожнім")]
        [Display(Name = "Праця")]
        public int WorkId { get; set; }
        [Required(ErrorMessage = "заповніть поле, ок?")]
        [Display(Name = "Учасник")]
        public int ParticipantId { get; set; }

        public virtual Participant Participant { get; set; }
        public virtual Work Work { get; set; }
    }
}
