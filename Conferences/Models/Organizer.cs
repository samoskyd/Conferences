using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Conferences
{
    public partial class Organizer
    {
        public Organizer()
        {
            Conferences = new HashSet<Conference>();
        }

        public int OrganizerId { get; set; }
        [Required(ErrorMessage = "це поле не можна залишати порожнім")]
        [Display(Name = "Ім'я")]
        public string FullName { get; set; }
        [Display(Name = "Дата реєстрації")]
        public DateTime? RegistrationDate { get; set; }
        [Required(ErrorMessage = "це поле не можна залишати порожнім")]
        [Display(Name = "Сфера діяльності")]
        public string Occupation { get; set; }

        public virtual ICollection<Conference> Conferences { get; set; }
    }
}
