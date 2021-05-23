using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Conferences
{
    public partial class Location
    {
        public Location()
        {
            Conferences = new HashSet<Conference>();
        }

        public int LocationId { get; set; }
        [Required(ErrorMessage = "заповніть поле, ок?")]
        [Display(Name = "Країна")]
        public string Country { get; set; }
        [Required(ErrorMessage = "це поле не можна залишати порожнім")]
        [Display(Name = "Місто")]
        public string City { get; set; }
        [Required(ErrorMessage = "кіко чоловік? а? впишіть максимальну місткість!")]
        [Display(Name = "Місткість")]
        public long Capacity { get; set; }

        public virtual ICollection<Conference> Conferences { get; set; }
    }
}
