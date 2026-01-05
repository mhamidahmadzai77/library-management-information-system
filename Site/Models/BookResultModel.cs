using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.Models
{
    public class BookResultModel
    {
        public long s_no { get; set; }
        public string publication_name { get; set; }
        public string publication_type { get; set; }
        public string translator { get; set; }
        public string branch { get; set; }
        public string book_type { get; set; }
        public bool permission { get; set; }
        public string publication_language { get; set; }
        public string authors { get; set; }

    }
}