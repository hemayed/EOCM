using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace EOCM.Models
{
    public class Field
    {
        [Key]
        [StringLength(255)]
        [Display(Name = "النشاط")]
        public string Field_ID { get; set; }
        [Required]
        [Display(Name = "النشاط")]
        public string Field_Name { get; set; }

        [Required]
        [Display(Name = "القطاع")]
        public string Sector_ID { get; set; }
        [ForeignKey("Sector_ID")]
        public virtual Sector Sector { get; set; }


        public virtual List<Product> Products { get; set; }
        public virtual ICollection<Cluster> Clusters { get; set; }
       
    }
}
