using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EOCM.Models
{
    public class ExportFlag
    {
        [Key]
        [StringLength(10)]
        public string ExportFlag_ID { get; set; }

        [Required]
        [StringLength(255)]
        public string ExportFlag_Name { get; set; }
    }
}