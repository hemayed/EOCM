﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace EOCM.Models
{
    public class Product
    {
        [Key]
        [StringLength(10)]
        public string Product_ID { get; set; }  
        [Required]
        [StringLength(255)]
        [Display(Name = "المنتج")]
        public string Product_Name { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "القطاع")]
        public string Sector_ID { get; set; }
        [ForeignKey("Sector_ID")]
        public virtual Sector Sector { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "النشاط")]
        public string Field_ID { get; set; }
        [ForeignKey("Field_ID")]
        public virtual Field Field { get; set; }
      
        public virtual ICollection<Cluster> Clusters { get; set; }

    }
}
