using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EOCM.Models
{
    public class Sector
    {
        [Key]
        [StringLength(10)]
        [Display(Name = "القطاع")]
        public string Sector_ID{ get; set; }
        [Required]
        [StringLength(255)]
        [Display(Name = "القطاع")]
        public string Sector_Name { get; set; }

        public virtual List<Field> Fields { get; set; }
        public virtual List<Product> Products { get; set; }
        public virtual ICollection<Cluster> Clusters { get; set; }

    }
}
