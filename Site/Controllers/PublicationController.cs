using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Site.Models;

namespace Site.Controllers
{
    public class PublicationController : Controller
    {
        // GET: Publication

        LibraryMISEntities db = new LibraryMISEntities();

       
        public ActionResult Index()
        {
            try
            {

                string sqlQuery = @"SELECT 
					edition.edition_id,
	                publication.s_no, 
	                publication.publication_name, 
	                publication.publication_type, 
	                publication.translator, 
	                publication.permission, 
	                publication.publication_language, 
	                branch.branch_name,
					segment.segment_no,
					edition.edition_no 
	
                FROM 
	                Publication publication 
	                INNER JOIN Branch branch ON branch.branch_id = publication.branch_id	
					INNER JOIN Segment segment ON segment.s_no= publication.s_no 
					INNER JOIN Edition edition ON edition.segment_id = segment.segment_id 
                ";

                // List to store query results
                List<PublicationResultModel> queryResults = new List<PublicationResultModel>();

                // Execute SQL query and fetch results
                using (SqlConnection connection = new SqlConnection("Data Source=.;Initial Catalog=LibraryMIS;Integrated Security=true"))
                {
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();


                    while (reader.Read())
                    {
                        // command for getting book author/authors
                        string allAuthors = "";
                        long serialNo = reader.GetInt64(reader.GetOrdinal("s_no"));
                        var query = from bookAuthor in db.BookAuthors
                                    join publication in db.Publications on bookAuthor.s_no equals publication.s_no
                                    join author in db.Authors on bookAuthor.author_id equals author.author_id
                                    where publication.s_no == serialNo
                                    select new
                                    {
                                        author.author_firstname,
                                        author.author_lastname
                                    };

                        var res = query;

                        int authorQuantity = 0;

                        foreach (var a in res)
                        {
                            ++authorQuantity;
                        }
                        if (authorQuantity == 1)
                        {
                            foreach (var a in res)
                            {
                                allAuthors += a.author_firstname + " " + a.author_lastname;
                            }
                        }
                        else
                        {
                            foreach (var a in res)
                            {
                                if (authorQuantity == 1)
                                {
                                    allAuthors += a.author_firstname + " " + a.author_lastname;

                                }
                                else
                                {
                                    allAuthors += a.author_firstname + " " + a.author_lastname + "، ";
                                    authorQuantity--;
                                }

                            }
                        }
                        // Map data from reader to result model
                        PublicationResultModel result = new PublicationResultModel
                        {
                            // Map data from reader to result model properties
                            edition_id = reader.GetInt64(reader.GetOrdinal("edition_id")),
                            s_no = reader.GetInt64(reader.GetOrdinal("s_no")),
                            publication_name = reader.GetString(reader.GetOrdinal("publication_name")),
                            publication_type = reader.GetString(reader.GetOrdinal("publication_type")),
                            translator = reader.GetString(reader.GetOrdinal("translator")),
                            branch = reader.GetString(reader.GetOrdinal("branch_name")),
                            permission = reader.GetBoolean(reader.GetOrdinal("permission")),
                            publication_language = reader.GetString(reader.GetOrdinal("publication_language")),
                            authors = allAuthors,
                            segment_no = reader.GetInt16(reader.GetOrdinal("segment_no")),
                            edition_no = reader.GetInt16(reader.GetOrdinal("edition_no"))
                        };

                        queryResults.Add(result);
                    }

                    reader.Close();
                }

                // Pass query results to view model
                var viewModel = new PublicationQueryResultModel
                {
                    QueryResults = queryResults
                };

                // Pass view model to view
                return View(viewModel);


            }
            catch (Exception ex)
            {

            }

            return View();
        }

        [HttpPost]
        public JsonResult PublicationDeletion(long id)
        {
            try
            {

                Publication publication = db.Publications.Where(p => p.s_no == id).FirstOrDefault();
                db.Publications.Remove(publication);
                int count = db.SaveChanges();
                if (count > 0)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PublicationDetails(long id, string publication_type)
        {
            if(publication_type == "Book")
            {
                return RedirectToAction("BookDetailsView","Book", new {id = id});
            }
            else if(publication_type == "Magazine")
            {
                return RedirectToAction("MagazineDetailsView", "Magazine", new { id = id });
            }
            else if(publication_type == "Thesis")
            {
                return RedirectToAction("ThesisDetailsView", "Thesis", new { id = id });
            }
            else if(publication_type == "Monograph")
            {
                return RedirectToAction("MonographDetailsView", "Monograph", new { id = id });
            }
            else
            {
                return View("Index");
            }

        }

    }
}