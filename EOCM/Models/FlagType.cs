using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EOCM.Models
{
    public class FlagType
    {
        [Key]
        [StringLength(10)]
        public string FlagType_ID { get; set; }

        [Required]
        [StringLength(255)]
        public string FlagType_Name { get; set; }
    }
}