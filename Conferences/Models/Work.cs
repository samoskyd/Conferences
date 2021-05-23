using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "заповніть поле, ок?")]
        [Display(Name = "Учасник")]
        public int ParticipantId { get; set; }
        [Required(ErrorMessage = "це поле не можна залишати порожнім")]
        [Display(Name = "Назва")]
        public string Title { get; set; }
        [Required(ErrorMessage = "заповніть поле, ок?")]
        [Display(Name = "Тема")]
        public string Topic { get; set; }
        [Display(Name = "Дата публікації")]
        public DateTime PublicationDate { get; set; }
        [Required(ErrorMessage = "заповніть поле, ок?")]
        [Display(Name = "Конференція")]
        public int ConferenceId { get; set; }

        public virtual ICollection<WorksAndParticipant> WorksAndParticipants { get; set; }
    }
}
