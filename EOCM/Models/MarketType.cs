using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EOCM.Models
{
    public class MarketType
    {
        [Key]
        [StringLength(10)]
        [Display(Name = "اتجاهات المبيعات/أهم الأسواق   ")]
        public string MarketType_ID { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "اتجاهات المبيعات/أهم الأسواق   ")]
        public string MarketType_Name { get; set; }
    }
}