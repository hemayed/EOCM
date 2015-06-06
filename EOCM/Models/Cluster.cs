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

        [Required]
        [Display(Name = "اسم التجمع")]
        public string Cluster_Name { get; set; }

        [Required]
        [Display(Name = "المحافظة")]
        public string Govt_ID { get; set; }
        [ForeignKey("Govt_ID")]
        public virtual Governorate Governorate { get; set; }

        [Display(Name = "المركز/المدينة")]
        public string District_ID { get; set; }
        public virtual District District { get; set; }

        [Display(Name = "القرية")]
        public string Village_ID { get; set; }
        public virtual Village Village { get; set; }

        [StringLength(255)]
        [Display(Name = "العنوان")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "القطاع")]
        public string Sector_ID { get; set; }
        [ForeignKey("Sector_ID")]
        public virtual Sector Sector { get; set; }

        [Required]
        [Display(Name = "النشاط")]
        public string Field_ID { get; set; }
        [ForeignKey("Field_ID")]
        public virtual Field Field { get; set; }

        [Required]
        [Display(Name = "المنتج")]
        public string Product_ID { get; set; }
        [ForeignKey("Product_ID")]
        public virtual Product Product { get; set; }

        [StringLength(1024)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "أهم منتجات التجمع")]
        public string Products { get; set; }

        [StringLength(1024)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "تفاصيل اكثر عن منتجات التجمع")]
        public string MoreProducts { get; set; }

        [Required]
        [Display(Name = "خط العرض")]
        public decimal Cluster_Lat { get; set; }

        [Required]
        [Display(Name = "خط الطول")]
        public decimal Cluster_Long { get; set; }

         [StringLength(255)]
        [Display(Name = "عدد العمال")]
        public string Cluster_EmpNum { get; set; }

         [StringLength(255)]
         [Display(Name = "نسبة عمالة المرأة ")]
         public string Cluster_EmpFemale { get; set; }

         [StringLength(255)]
        [Display(Name = "عدد شركات /ورش /وحدات انتاجية")]
        public string Cluster_ShopNum { get; set; }

         [StringLength(255)]
        [Display(Name = "نسبة المشاريع الرسمية في التجمع")]
        public string OfficalProjects { get; set; }

         [StringLength(255)]
        [Display(Name = "نسبة المشاريع الغير رسمية في التجمع")]
        public string NonOfficalProjects { get; set; }

         [StringLength(255)]
         [Display(Name = "نسبة الشركات المتناهية الصغر والصغيرة والمتوسطة والكبيرة ")]
         public string CompanyPercent { get; set; }

         [StringLength(1024)]
         [DataType(DataType.MultilineText)]
        [Display(Name = " (طبيعة هذا التجمع ( تكامل رأسى  / تكامل أفقى / مركزى")]
        public string ClusterNature { get; set; }

         [StringLength(1024)]
         [DataType(DataType.MultilineText)]
        [Display(Name = " (نوع التجمع (استهداف نفس السوق / استخدام نفس الموارد / وجود نفس البنية الأساسية والمرافق")]
        public string ClusterType { get; set; }

         [StringLength(1024)]
         [DataType(DataType.MultilineText)]
        [Display(Name = "الشركات / الهيئات / الجهات الداعمة للتجمع")]
        public string SupportingOrg { get; set; }

         [StringLength(1024)]
         [DataType(DataType.MultilineText)]
        [Display(Name = "أهم التحديات التي يواجهها التجمع")]
        public string Challenges { get; set; }

         [StringLength(1024)]
         [DataType(DataType.MultilineText)]
         [Display(Name = "تفاصيل اكثر عن التحديات التي يواجهها التجمع")]
         public string MoreChallenges { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "صورة المنتج")]
        public string Cluster_ProductImage { get; set; }
        
        [DataType(DataType.Upload)]
        [Display(Name = "صورة عملية الانتاج")]
        public string Cluster_ProcessImage { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "دراسة عن التجمع ")]
        public string Cluster_StudyFile1 { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "معلومات اضافية عن التجمع ")]
        public string Cluster_StudyFile2 { get; set; }

        [DataType(DataType.Url)]
        [Display(Name = " روابط لصفحات خارجية ذات صلة")]
        public string Cluster_DetailPage1 { get; set; }

        [Display(Name = " رابط 2")]
        [DataType(DataType.Url)]
        public string Cluster_DetailPage2 { get; set; }
        [Display(Name = " رابط 3")]
        [DataType(DataType.Url)]
        public string Cluster_DetailPage3 { get; set; }

        [StringLength(255)]
        [Display(Name = " أهم الأسواق ")]
        public string Market { get; set; }

        [StringLength(255)]
        [Display(Name = " موسمية المنتجات المباعة ")]
        public string ProductSeason { get; set; }

        [StringLength(255)]
        [Display(Name = " حجم / نسبة التصدير ")]
        public string ExportVolume { get; set; }

        [StringLength(255)]
        [Display(Name = " مستوى الدخل ")]
        public string Income { get; set; }

    }
}