using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.ViewModel
{
    public class UploadImage
    {
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public HttpPostedFileWrapper image { get; set; }
        public string level { get; set; }

    }
}