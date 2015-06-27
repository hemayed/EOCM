using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EOCM.Models
{
    public class Governorate
    {
        [Key]
        [StringLength(10)]
        [Display(Name = "المحافظة")]
        public string Govt_ID { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "المحافظة")]
        public string Govt_Name { get; set; }

        [Required]
        [Display(Name = "خط العرض")]
        [Range(22, 32, ErrorMessage = "يجب ان يكون خط العرض بين 22 و 32")]
        public decimal Govt_Lat { get; set; }

        [Required]
        [Display(Name = "خط الطول")]
        [Range(24, 37, ErrorMessage = "يجب ان يكون خط الطول بين 24 و 37")]
        public decimal Govt_Long { get; set; }

        //public virtual List<District> Districts { get; set; }
        //public virtual List<Village> Villages { get; set; }
        public virtual ICollection<Cluster> Clusters { get; set; }
          
       

       }
}
