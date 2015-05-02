using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EOCM.Models
{
    public class ClusterMapViewModel
    {
        public int Cluster_Num { get; set; }
        public string Cluster_ID { get; set; }
        public decimal Cluster_Lat { get; set; }
        public decimal Cluster_Long { get; set; }
        public string Cluster_Name { get; set; }
        public string Cluster_ProductImage { get; set; }  
        public string Cluster_DetailPage { get; set; }

        public string Cluster_Info1 { get; set; }
        public string Cluster_Info2 { get; set; }
        public string Cluster_Info3 { get; set; }

    }
}