using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EOCM.Models
{
    public class Cluster
    {
        [Key]
        [StringLength(10)]
        public string Cluster_ID { get; set; }

        [Required]
        [Display(Name = "اسم التجمع")]
        [StringLength(255)]
        public string Cluster_Name { get; set; }

        [Required]
        [Display(Name = "المحافظة")]
        [StringLength(10)]
        public string Govt_ID { get; set; }
        [ForeignKey("Govt_ID")]
        public virtual Governorate Governorate { get; set; }

        [Display(Name = "المركز/المدينة")]
        [StringLength(10)]
        public string District_ID { get; set; }
        public virtual District District { get; set; }

        [Display(Name = "القرية")]
        [StringLength(10)]
        public string Village_ID { get; set; }
        public virtual Village Village { get; set; }

        [StringLength(255)]
        [Display(Name = "العنوان")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "القطاع")]
        [StringLength(10)]
        public string Sector_ID { get; set; }
        [ForeignKey("Sector_ID")]
        public virtual Sector Sector { get; set; }

        [Required]
        [Display(Name = "النشاط")]
        [StringLength(10)]
        public string Field_ID { get; set; }
        [ForeignKey("Field_ID")]
        public virtual Field Field { get; set; }

        [Required]
        [Display(Name = "المنتج")]
        [StringLength(10)]
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
        [Range(22, 32, ErrorMessage = "يجب ان يكون خط العرض بين 22 و 32")]
        public decimal Cluster_Lat { get; set; }

        [Required]
        [Display(Name = "خط الطول")]
        [Range(24, 37, ErrorMessage = "يجب ان يكون خط الطول بين 24 و 37")]
        public decimal Cluster_Long { get; set; }

        [Required]
        [DefaultValue(0)]
        [Range(0, int.MaxValue, ErrorMessage = "يجب ان يكون العدد اكبر من 0")]
        [Display(Name = "عدد العمال -الحد الادنى")]
        public int  Cluster_EmpNumMin { get; set; }

        [Required]
        [DefaultValue(0)]
        [Range(0, int.MaxValue, ErrorMessage = "يجب ان يكون العدد اكبر من 0")]
         [Display(Name = "  عدد العمال -الحد الاعلى")]
        public int Cluster_EmpNumMax { get; set; }

        [DefaultValue(0)]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:P0}")]
        [RegularExpression(@"[0-9])?$", ErrorMessage = " ادخل نسبة مئوية")]
         [Display(Name = "نسبة عمالة المرأة ")]
        public int? Cluster_EmpFemale { get; set; }

        [Required]
        [DefaultValue(0)]
        [Range(0, int.MaxValue, ErrorMessage = "يجب ان يكون العدد اكبر من 0")]
        [Display(Name = "عدد الوحدات الانتاجية - الحد الادنى")]
        public int Cluster_ShopNumMin { get; set; }

        [Required]
        [DefaultValue(0)]
        [Range(0, int.MaxValue, ErrorMessage = "يجب ان يكون العدد اكبر من 0")]
        [Display(Name = "عدد الوحدات الانتاجية - الحد الاعلى")]
        public int Cluster_ShopNumMax { get; set; }

         [StringLength(255)]
        [Display(Name = "نسبة المشاريع الرسمية في التجمع")]
        public string OfficalProjects { get; set; }

         [StringLength(255)]
        [Display(Name = "نسبة المشاريع الغير رسمية في التجمع")]
        public string NonOfficalProjects { get; set; }

         [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:P0}")]
         [RegularExpression(@"[0-9])?$", ErrorMessage = " ادخل نسبة مئوية")]
         [Display(Name = "نسبة الشركات المتناهية الصغر (أى عدد العمالة من 1 إلى 4)  ")]
         public int? CompanyPercent1 { get; set; }

         [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:P0}")]
         [RegularExpression(@"[0-9])?$", ErrorMessage = " ادخل نسبة مئوية")]
         [Display(Name = "نسبة الشركات الصغيرة (أى عدد العمالة من 5 إلى 49)  ")]
         public int? CompanyPercent2 { get; set; }

         [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:P0}")]
         [RegularExpression(@"[0-9])?$", ErrorMessage = " ادخل نسبة مئوية")]
         [Display(Name = "نسبة الشركات المتوسطة (أى عدد العمالة من 50 إلى 99)  ")]
         public int? CompanyPercent3 { get; set; }

         [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:P0}")]
         [RegularExpression(@"[0-9])?$", ErrorMessage = " ادخل نسبة مئوية")]
         [Display(Name = "نسبة الشركات الكبيرة (أى عدد العمالة أكثر من 100) ")]
         public int? CompanyPercent4 { get; set; }

       
         [StringLength(10)]
        [Display(Name = "طبيعة هذا التجمع (تكامل رأسي / تكامل افقي / مركزي)")]
         public string ClusterNature_ID { get; set; }
         [ForeignKey("ClusterNature_ID")]
         public virtual ClusterNature ClusterNature { get; set; }

         [StringLength(10)]
         [Display(Name = " سبب تواجد التجمع (استهداف نفس السوق / استخدام نفس الموارد / وجود نفس البنية الأساسية والمرافق)")]
         public string ClusterType_ID { get; set; }
         [ForeignKey("ClusterType_ID")]
         public virtual ClusterType ClusterType { get; set; }

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

         [StringLength(1024)]
        [DataType(DataType.Upload)]
        [Display(Name = "صورة المنتج")]
        public string Cluster_ProductImage { get; set; }

        [StringLength(1024)]
        [DataType(DataType.Upload)]
        [Display(Name = "صورة عملية الانتاج")]
        public string Cluster_ProcessImage { get; set; }

         [StringLength(1024)]
        [DataType(DataType.Upload)]
        [Display(Name = "دراسة عن التجمع ")]
        public string Cluster_StudyFile1 { get; set; }

         [StringLength(1024)]
        [DataType(DataType.Upload)]
        [Display(Name = "معلومات اضافية عن التجمع ")]
        public string Cluster_StudyFile2 { get; set; }

         [StringLength(1024)]
        [DataType(DataType.Url)]
        [Display(Name = " روابط لصفحات خارجية ذات صلة")]
        public string Cluster_DetailPage1 { get; set; }

         [StringLength(1024)]
        [Display(Name = " رابط 2")]
        [DataType(DataType.Url)]
        public string Cluster_DetailPage2 { get; set; }

         [StringLength(1024)]
        [Display(Name = " رابط 3")]
        [DataType(DataType.Url)]
        public string Cluster_DetailPage3 { get; set; }

        [StringLength(10)]
        [Display(Name = " اتجاهات المبيعات/أهم الأسواق   ")]
        public string MarketType_ID { get; set; }
        [ForeignKey("MarketType_ID")]
        public virtual MarketType MarketType { get; set; }

        [StringLength(10)]
        [Display(Name = " موسمية المنتجات المباعة ")]
        public string ProductSeason_ID { get; set; }
        [ForeignKey("ProductSeason_ID")]
        public virtual ProductSeason ProductSeason { get; set; }

        [StringLength(1024)]
        [DataType(DataType.MultilineText)]
        [Display(Name = " تفاصيل عن موسمية المنتاجات المباعة ")]
        public string ProductSeasonDetail { get; set; }


        [UIHint("YesNo")]
        [Display(Name = " هل يقوم التجمع بالتصدير ")]
        public Nullable<bool> ExportFlag { get; set; }

        [StringLength(255)]
        [Display(Name = " نسبة التصدير ")]
        public string ExportVolume { get; set; }

        [StringLength(255)]
        [Display(Name = " متوسط مستوى الدخل  ")]
        public string Income { get; set; }

        [StringLength(1024)]
        [DataType(DataType.MultilineText)]
        [Display(Name = " بيانات إضافية عن التجمع ")]
        public string AdditionalInfo { get; set; }
    }
}