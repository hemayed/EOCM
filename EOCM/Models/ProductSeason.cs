using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;


namespace EOCM.Models
{
    public class ProductSeason
    {
        [Key]
        [StringLength(10)]
        [Display(Name = "موسمية المنتجات المباعة ")]
        public string ProductSeason_ID { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "موسمية المنتجات المباعة ")]
        public string ProductSeason_Name { get; set; }
    }
}