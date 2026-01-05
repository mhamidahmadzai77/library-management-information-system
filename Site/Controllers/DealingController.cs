using Site.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Site.Controllers
{
    public class DealingController : Controller
    {
        LibraryMISEntities db = new LibraryMISEntities();
        SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=LibraryMIS;Integrated Security=true");

        public string allAuthors;
        public long serialNo;
        public long editionId;

        // GET: Dealing
        public ActionResult Index()
        {

            try
            {

                string sqlQuery = @"SELECT 
	               publication.s_no, 
	               publication.publication_name,  
				   segment.segment_id,
				   segment.segment_no, 
				   edition.edition_id, 
				   edition.edition_no, 
				   dealing.dealing_id, 
				   dealing.issue_date, 
				   dealing.return_date,
				   dealing.paid_money, 
				   dealing.returned, 
				   person.person_id,
				   person.person_idCard,
				   person.person_firstname,
				   person.person_lastname 
	 
                FROM  
	                Publication publication  
	                INNER JOIN Segment segment ON segment.s_no = publication.s_no 
	                INNER JOIN Edition edition ON edition.segment_id = segment.segment_id 
	                INNER JOIN Dealing dealing ON dealing.edition_id = edition.edition_id 
	                INNER JOIN Person person ON person.person_id = dealing.person_id
                    ORDER BY dealing.dealing_id DESC";

                // List to store query results
                List<DealingResultModel> queryResults = new List<DealingResultModel>();

                // Execute SQL query and fetch results
                using (SqlConnection connection = new SqlConnection("Data Source=.;Initial Catalog=LibraryMIS;Integrated Security=true"))
                {
                    SqlCommand command = new SqlCommand(sqlQuery, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();


                    while (reader.Read())
                    {



                        // Map data from reader to result model
                        DealingResultModel result = new DealingResultModel
                        {
                            // Map data from reader to result model properties
                            s_no = reader.GetInt64(reader.GetOrdinal("s_no")),
                            publication_name = reader.GetString(reader.GetOrdinal("publication_name")),
                            person_id = reader.GetInt64(reader.GetOrdinal("person_id")),
                            person_idCard = reader.GetString(reader.GetOrdinal("person_idCard")),
                            person_firstname = reader.GetString(reader.GetOrdinal("person_firstname")),
                            person_lastname = reader.GetString(reader.GetOrdinal("person_lastname")),
                            segment_id = reader.GetInt64(reader.GetOrdinal("segment_id")),
                            segment_no = reader.GetInt16(reader.GetOrdinal("segment_no")),
                            edition_id = reader.GetInt64(reader.GetOrdinal("edition_id")),
                            edition_no = reader.GetInt16(reader.GetOrdinal("edition_no")),
                            dealing_id = reader.GetInt64(reader.GetOrdinal("dealing_id")),
                            issue_date = reader.GetDateTime(reader.GetOrdinal("issue_date")),
                            return_date = reader.GetDateTime(reader.GetOrdinal("return_date")),
                            paid_money = reader.GetInt32(reader.GetOrdinal("paid_money")),
                            returned = reader.GetBoolean(reader.GetOrdinal("returned"))
                        };

                        queryResults.Add(result);
                    }

                    reader.Close();
                }

                // Pass query results to view model
                var viewModel = new DealingQueryResultViewModel
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

        public ActionResult DealingRegistrationView()
        {
            // Getting Data for person selection table
            List<Person> data = db.People.ToList();
            ViewBag.Person = data;

            // Getting data for book selection table
            try
            {

                string sqlQuery = @"SELECT 
	                publication.s_no,
	                edition.edition_id,
	                publication.publication_name,
	                publication.publication_type,
                    publication.permission, 
	                segment.segment_no,
	                edition.edition_no, 
                    edition.publication_quantity 
                 FROM Publication publication 
                 INNER JOIN Segment segment ON segment.s_no = publication.s_no 
                 INNER JOIN Edition edition ON edition.segment_id = segment.segment_id ";

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

                        serialNo = reader.GetInt64(reader.GetOrdinal("s_no"));

                        allAuthors = "";
                       

                        // Open connection
                        con.Open();

                        string quer = @"select author.author_id, author.author_firstname, author.author_lastname from BookAuthor bookAuthor
                            inner join Author author on author.author_id = bookAuthor.author_id
                            where bookAuthor.s_no = " + serialNo;

                        SqlDataAdapter da = new SqlDataAdapter(quer, con);
                        DataTable bookAuthors = new DataTable();
                        da.Fill(bookAuthors);
                        //Close connection
                        con.Close();

                        int authorQuantity = 0;
                        authorQuantity = bookAuthors.Rows.Count;


                        foreach (DataRow row in bookAuthors.Rows)
                        {
                            int columnNo = 0;
                            foreach (var col in row.ItemArray)
                            {
                                columnNo++;

                                if (authorQuantity == 1)
                                {
                                    if (columnNo == 2)
                                    {
                                        allAuthors += col + " ";

                                    }
                                    else if (columnNo == 3)
                                    {
                                        allAuthors += col + " ";
                                    }


                                }
                                else
                                {
                                   
                                    if (columnNo == 2)
                                    {
                                        allAuthors += col + " ";

                                    }
                                    else if (columnNo == 3)
                                    {
                                        allAuthors += col + ", ";
                                    }
                                }

                            }
                            authorQuantity--;

                        }


                        editionId = reader.GetInt64(reader.GetOrdinal("edition_id"));

                        int availableQuantity = 0;
                        availableQuantity = db.Dealings.Where(d => d.edition_id == editionId && d.returned == false).Count();
                        int totalQuantity = Convert.ToInt32(reader.GetInt16(reader.GetOrdinal("publication_quantity")));
                        availableQuantity = totalQuantity - availableQuantity;

                        

                        // Map data from reader to result model
                        PublicationResultModel result = new PublicationResultModel
                        {

                            // Map data from reader to result model properties
                            s_no = reader.GetInt64(reader.GetOrdinal("s_no")),
                            publication_name = reader.GetString(reader.GetOrdinal("publication_name")),
                            publication_type = reader.GetString(reader.GetOrdinal("publication_type")),
                            permission = reader.GetBoolean(reader.GetOrdinal("permission")),
                            authors = allAuthors,
                            segment_no = reader.GetInt16(reader.GetOrdinal("segment_no")),
                            edition_id = reader.GetInt64(reader.GetOrdinal("edition_id")),
                            edition_no = reader.GetInt16(reader.GetOrdinal("edition_no")),
                            publication_quantity = reader.GetInt16(reader.GetOrdinal("publication_quantity")),
                            availableQuantity = availableQuantity
                            
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
        public JsonResult DealingRegistration(Dealing dealing)
        {

            try
            {
                db.Dealings.Add(dealing);
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
                return Json(ex, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult bookReturned(long dealing_id)
        {
            try
            {
                Dealing dealing = db.Dealings.Where(d => d.dealing_id == dealing_id).FirstOrDefault();
                dealing.returned = true;
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
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }

    }

}