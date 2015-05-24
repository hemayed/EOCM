using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EOCM.Models
{
    public class ClusterData
    {
       
        public string Cluster_ID { get; set; }
       
        public string Cluster_Name { get; set; }

        public int Govt_ID { get; set; }
        public string Govt_Name { get; set; }

        public int District_ID { get; set; }
        public string District_Name { get; set; }

        public int Village_ID { get; set; }
        public string Village_Name { get; set; }
       
        public string Address { get; set; }

        public int Sector_ID { get; set; }
       
      
        public string Products { get; set; }

        public decimal Cluster_Lat { get; set; }
        public decimal Cluster_Long { get; set; }

        public string Cluster_Info1 { get; set; }
        public string Cluster_Info2 { get; set; }
        public string Cluster_Info3 { get; set; }
        public string Cluster_Info4 { get; set; }

     
        public int OfficalProjects { get; set; }

       
        public int NonOfficalProjects { get; set; }

       
        public string ClusterNature { get; set; }

    
        public string ClusterType { get; set; }

       
        public string SupportingOrg { get; set; }

       
        public string Challenges { get; set; }

      
        public string Cluster_ProductImage { get; set; }

       
        public string Cluster_ProcessImage { get; set; }

       
        public string Cluster_DetailPage { get; set; }

    }
}