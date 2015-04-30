using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace EOCM.Models
{
    public class District
    {
        [Key]
        [StringLength(255)]
        [Display(Name = "المركز")]
        public string District_ID { get; set; }
         [Display(Name = "المركز")]
        public string District_Name { get; set; }

        [Display(Name = "المحافظة")]
        public string Govt_ID { get; set; }
        [ForeignKey("Govt_ID")]
        public virtual Governorate Governorate { get; set; }
        
        public virtual List<Village> Villages { get; set; }
        public virtual ICollection<Cluster> Clusters { get; set; }
               
    }
}
