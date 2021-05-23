using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "заповніть поле, ок?")]
        [Display(Name = "Ім'я")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "це поле не можна залишати порожнім")]
        [Display(Name = "Дата народження")]
        public DateTime BirthDate { get; set; }
        [Display(Name = "Дата реєстрації")]
        public DateTime? RegistrationDate { get; set; }
        [Required(ErrorMessage = "це поле не можна залишати порожнім")]
        [Display(Name = "Сфера діяльності")]
        public string Occupation { get; set; }
        [Required(ErrorMessage = "заповніть поле, ок?")]
        [Display(Name = "Інститут")]
        public string Institution { get; set; }

        public virtual ICollection<ConferencesAndParticipant> ConferencesAndParticipants { get; set; }
        public virtual ICollection<WorksAndParticipant> WorksAndParticipants { get; set; }
    }
}
