using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EOCM.Models
{
   
    public class ClusterMapViewModel
    {
       
        public List<GovtData> GovtData {get; set;}
        public List<ClusterData> ClusterData { get; set; }


    }
}