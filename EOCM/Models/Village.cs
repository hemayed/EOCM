﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace EOCM.Models
{
    public class Village
    {
        [Key]
        [StringLength(255)]
        
        [Display(Name = "القرية")]
        public string Village_ID { get; set; }
        [Required]
        [Display(Name = "القرية")]
        public string Village_Name { get; set; }
        [Required]
        [Display(Name = "المحافظة")]
        public string Govt_ID { get; set; }
        [ForeignKey("Govt_ID")]
        public virtual Governorate Governorate { get; set; }
        [Required]
        [Display(Name = "المركز/المدينة")]
        public string District_ID { get; set; }
        [ForeignKey("District_ID")]
        public virtual District District { get; set; }
        
       public virtual ICollection<Cluster> Clusters { get; set; }

    }
}
