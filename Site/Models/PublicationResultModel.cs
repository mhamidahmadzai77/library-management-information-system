using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Site.Models
{
    public class PublicationResultModel
    {
        public long s_no { get; set; }
        public string ISBN { get; set; }
        public string DDC_classificationNo { get; set; }
        public string LLC_classificationNo { get; set; }
        public string publication_name { get; set; }
        public string publication_type { get; set; }
        public string translator { get; set; }
        public string branch { get; set; }
        public bool permission { get; set; }
        public string publication_language { get; set; }
        public string publication_description { get; set; }
        public string authors { get; set; }
        public string author_description { get; set; }
        public long segment_id { get; set; }
        public short segment_no { get; set; }
        public long edition_id { get; set; }
        public short edition_no { get; set; }
        public int availableQuantity { get; set; }
        public short publication_quantity { get; set; }
        public short publication_pages { get; set; }
        public short cd_quantity { get; set; }
        public string registration_date_type { get; set; }
        public int registration_year { get; set; }
        public int registration_month { get; set; }
        public int registration_day { get; set; }
        public string publication_date_type { get; set; }
        public int publication_year { get; set; }
        public int publication_month { get; set; }
        public int publication_day { get; set; }
        public short cupboard_no { get; set; }
        public short cell_no { get; set; }
        public long book_id { get; set; }
        public string book_type { get; set; }
        public long bookAndMagazine_id { get; set; }
        public short publication_no { get; set; }
        public string publication_place { get; set; }
        public string publisher_name { get; set; }
        public string supervisor_firstname { get; set; }
        public string supervisor_lastname { get; set; }
        public string student_registration_year { get; set; }
        public string student_graduation_year { get; set; }
        public DateTime defence_data { get; set; }
        public string mark { get; set; }
        public long thesis_id { get; set; }
        public long monograph_thesis_id { get; set; }
        public string graduation_period { get; set; }
        public string internal_conflict { get; set; }
        public string external_conflict { get; set; }
        
    }
}