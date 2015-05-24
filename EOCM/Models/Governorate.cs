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
        [StringLength(255)]
        [Display(Name = "المحافظة")]
        public string Govt_ID { get; set; }

        [Display(Name = "المحافظة")]
        public string Govt_Name { get; set; }

        [Display(Name = "خط الطول")]
        public decimal Govt_Lat { get; set; }
        [Display(Name = "خط العرض")]
        public decimal Govt_Long { get; set; }

        //public virtual List<District> Districts { get; set; }
        //public virtual List<Village> Villages { get; set; }
        public virtual ICollection<Cluster> Clusters { get; set; }
          
       

       }
}
