using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EOCM.Models
{
    public class Cluster
    {
        [Key]
        [StringLength(255)]
        public string Cluster_ID { get; set; }
        [Display(Name = "اسم التجمع")]
        public string Cluster_Name { get; set; }

        [Display(Name = "المحافظة")]
        public string Govt_ID { get; set; }
        [ForeignKey("Govt_ID")]
        public virtual Governorate Governorate { get; set; }

        [Display(Name = "المركز")]
        public string District_ID { get; set; }
        [ForeignKey("District_ID")]
        public virtual District District { get; set; }

        [Display(Name = "القرية")]
        public string Village_ID { get; set; }
        [ForeignKey("Village_ID")]
        public virtual Village Village { get; set; }

        [Display(Name = "القطاع")]
        public string Sector_ID { get; set; }
        [ForeignKey("Sector_ID")]
        public virtual Sector Sector { get; set; }

        [Display(Name = "النشاط")]
        public string Field_ID { get; set; }
        [ForeignKey("Field_ID")]
        public virtual Field Field { get; set; }

        [Display(Name = "المنتج")]
        public string Product_ID { get; set; }
        [ForeignKey("Product_ID")]
        public virtual Product Product { get; set; }
        
        [Display(Name = "خط الطول")]
        public decimal Cluster_Lat { get; set; }
        [Display(Name = "خط العرض")]
        public decimal Cluster_Long { get; set; }
        [Display(Name = "عدد العمال")]
        public Nullable<decimal> Cluster_EmpNum { get; set; }
        [Display(Name = "عدد الورش")]
        public Nullable<decimal> Cluster_ShopNum { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "صورة المنتج")]
        public string Cluster_ProductImage { get; set; }
        
        [DataType(DataType.Upload)]
        [Display(Name = "صورة عملية الانتاج")]
        public string Cluster_ProcessImage { get; set; }
        
        [DataType(DataType.Url)]
        [Display(Name = "رابط صفحة التفاصيل")]
        public string Cluster_DetailPage { get; set; }

        
    }
}