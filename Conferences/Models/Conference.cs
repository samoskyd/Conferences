using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "заповніть поле, ок?")]
        [Display(Name = "Назва")]
        public string Title { get; set; }
        [Required(ErrorMessage = "це поле не можна залишати порожнім")]
        [Display(Name = "Ціль")]
        public string Aim { get; set; }
        [Required(ErrorMessage = "заповніть поле, ок?")]
        [Display(Name = "Тема")]
        public string Topic { get; set; }
        [Required(ErrorMessage = "це поле не можна залишати порожнім")]
        [Display(Name = "Форма проведення")]
        public int FormId { get; set; }
        [Required(ErrorMessage = "заповніть поле, ок?")]
        [Display(Name = "Вимоги до робіт")]
        public string RequirementsForWorks { get; set; }
        [Required(ErrorMessage = "це поле не можна залишати порожнім")]
        [Display(Name = "Вимоги до учасників")]
        public string RequirementsForParticipants { get; set; }
        [Required(ErrorMessage = "заповніть поле, ок?")]
        [Display(Name = "Ціна")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "заповніть поле, ок?")]
        [Display(Name = "Дата та час проведення")]
        public DateTime DateAndTime { get; set; }
        [Required(ErrorMessage = "це поле не можна залишати порожнім")]
        [Display(Name = "Організатор")]
        public int OrganizerId { get; set; }
        [Required(ErrorMessage = "це поле не можна залишати порожнім")]
        [Display(Name = "Місце проведення")]
        public int LocationId { get; set; }

        public virtual Form Form { get; set; }
        public virtual Location Location { get; set; }
        public virtual Organizer Organizer { get; set; }
        public virtual ICollection<ConferencesAndParticipant> ConferencesAndParticipants { get; set; }
    }
}
