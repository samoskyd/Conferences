using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Conferences
{
    public partial class Form
    {
        public Form()
        {
            Conferences = new HashSet<Conference>();
        }

        public int FormId { get; set; }
        [Required(ErrorMessage = "заповніть поле, ок?")]
        [Display(Name = "Назва")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "це поле не можна залишати порожнім")]
        [Display(Name = "Максимальна місткість")]
        public string AvailableAudienceSize { get; set; }

        public virtual ICollection<Conference> Conferences { get; set; }
    }
}
