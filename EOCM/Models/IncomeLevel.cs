using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EOCM.Models
{
    public class IncomeLevel
    {
        [Key]
        [StringLength(10)]
        public string IncomeLevel_ID { get; set; }

        [Required]
        [StringLength(255)]
        public string IncomeLevel_Name { get; set; }
    }
}