using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Site.Models;
using System.Data.SqlClient;
using System.Data;

namespace Site.Controllers
{
    public class ReportController : Controller
    {
        LibraryMISEntities db = new LibraryMISEntities();
        // GET: Report

        // Database connection
        SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=LibraryMIS;Integrated Security=true");
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NotReturnedBook()
        {
            try
            {

                ReportDocument rd = new ReportDocument();
                rd.Load(Path.Combine(Server.MapPath("~/Report"), "NotReturnedBook.rpt"));
                rd.SetDataSource(db.NotReturnedBooks.ToList());
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "باقیداره مراجعین راپور.pdf");
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public ActionResult BranchVoiceReport()
        {
            try
            {
                string query = "SELECT  CASE  WHEN publication.publication_type = 'Book' THEN N'کتاب' WHEN publication.publication_type = 'Magazine' THEN N'مجله' WHEN publication.publication_type = 'Thesis' THEN N'تېزس' WHEN publication.publication_type = 'Monograph' THEN N'مونوګراف' END AS publication_type ,  SUM(edition.publication_quantity) AS publication_quantity FROM Publication publication INNER JOIN Segment segment ON segment.s_no = publication.s_no INNER JOIN Edition edition ON edition.segment_id = segment.segment_id INNER JOIN Branch branch ON branch.branch_id = publication.branch_id GROUP BY publication.publication_type";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt); if (dt.Rows.Count > 0)
                {
                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Report"), "BranchVoiceReport.rpt"));
                    rd.SetDataSource(dt);
                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "ډ ټولو شعبو راپور.pdf");
                }
                
            }
            catch (Exception ex)
            {

                throw;
            }
            return View("Index");
        }

        public ActionResult NewspaperReport()
        {
            try
            {
                string query = "SELECT newspaper_name, publisher_name, SUM(quantity) AS quantity FROM Newspaper  GROUP BY newspaper_name, publisher_name";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt); if (dt.Rows.Count > 0)
                {
                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Report"), "NewspaperReport.rpt"));
                    rd.SetDataSource(dt);
                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "د اخبارونو راپور.pdf");
                }
                
            }
            catch (Exception ex)
            {

                throw;
            }

            return View("Index");
        }

        public ActionResult TotalPublicatoinReport()
        {
            try
            {
                string query = "SELECT publication.publication_name, CASE WHEN publication.publication_type = 'Book' THEN N'کتاب' WHEN publication.publication_type = 'Magazine' THEN N'مجله' WHEN publication.publication_type = 'Thesis' THEN N'تېزس' WHEN publication.publication_type = 'Monograph' THEN N'مونوګراف' END AS publicationType , 	SUM(edition.publication_quantity) AS publication_quantity FROM Publication publication INNER JOIN Segment segment ON segment.s_no = publication.s_no INNER JOIN Edition edition ON edition.segment_id = segment.segment_id GROUP BY publication.publication_name , publication.publication_type ";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt); if (dt.Rows.Count > 0)
                {
                    ReportDocument rd = new ReportDocument();
                    rd.Load(Path.Combine(Server.MapPath("~/Report"), "TotalPublicatoinReport.rpt"));
                    rd.SetDataSource(dt);
                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                    stream.Seek(0, SeekOrigin.Begin);
                    return File(stream, "application/pdf", "د ټولو کتابونو مجلو تېزس او مونوګرافونو راپور.pdf");
                }

                return View("Index");

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}