using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EOCM.Models
{
    public class ClusterNature
    {
        [Key]
        [StringLength(10)]
        [Display(Name = "طبيعة التجمع")]
        public string ClusterNature_ID { get; set; }

        [Required]
        [StringLength(512)]
        [Display(Name = "طبيعة التجمع")]
        public string ClusterNature_Name { get; set; }
    }
}