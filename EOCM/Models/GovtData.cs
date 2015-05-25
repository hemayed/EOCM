using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EOCM.Models
{
    public class GovtData
    {
        
            public string Govt_ID { get; set; }
            public string Govt_Name { get; set; }
            public decimal Govt_Lat { get; set; }
            public decimal Govt_Long { get; set; }
            public int Cluster_Num { get; set; }
            public  int[] Sector_Num {get; set;}
    }
}