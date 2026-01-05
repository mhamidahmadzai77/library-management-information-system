using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.Models
{
    public class ViewBooks 
    {
        public string s_no { get; set; }
        public string publication_name { get; set; }
        public string publication_type { get; set; }
        public string translator { get; set; }
        public string branch { get; set; }
        public bool permission { get; set; }
        public string publication_language { get; set; }
        public string author_firstname { get; set; }
        public string author_lastname { get; set; }

        
    }
}