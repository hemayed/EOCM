using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EOCM.Models
{
    public class ClusterType
    {
        [Key]
        [StringLength(10)]
        [Display(Name = "سبب تواجد التجمع ")]
        public string ClusterType_ID { get; set; }

        [Required]
        [StringLength(512)]
        [Display(Name = "سبب تواجد التجمع ")]
        public string ClusterType_Name { get; set; }
    }
}