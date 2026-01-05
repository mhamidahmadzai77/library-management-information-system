using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.Models
{
    public class DealingResultModel
    {   
        public long person_id { get; set; }
        public string person_idCard { get; set; }
        public string person_firstname { get; set; }
        public string person_lastname { get; set; }
        public long s_no { get; set; }
        public long segment_id { get; set; }
        public short segment_no { get; set; }
        public long edition_id { get; set; }
        public short edition_no { get; set; }
        public string publication_name { get; set; }
        public long dealing_id { get; set; }
        public DateTime issue_date { get; set; }
        public DateTime return_date { get; set; }
        public int paid_money { get; set; }
        public bool returned { get; set; }
    }
}