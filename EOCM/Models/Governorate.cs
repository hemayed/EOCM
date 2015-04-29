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
        public string Govt_ID { get; set; }

        [Display(Name = "المحافظة")]
        public string Govt_Name { get; set; }

        public virtual List<District> Districts { get; set; }
        public virtual ICollection<Cluster> Clusters { get; set; }
          
       

       }
}
