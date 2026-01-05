using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.Models
{
    public class combinedPersonStudentViewModel
    {
        public long person_id { get; set; }
        public string person_idCard { get; set; }
        public string person_firstname { get; set; }
        public string person_lastname { get; set; }
        public string person_father_name { get; set; }

        public string gender { get; set; }
        public string university { get; set; }
        public string faculty { get; set; }
        public string department { get; set; }
        /* public long id { get; set; }*/
        public short semester { get; set; }
        public string phone_number { get; set; }
        public string email { get; set; }
    }
}