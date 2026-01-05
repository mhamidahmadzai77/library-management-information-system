using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Site.Models;

namespace Site.Controllers
{
    public class ThesisController : Controller
    {
        // GET: Thesis


        LibraryMISEntities db = new LibraryMISEntities();

        // Variables
        int segemntNo = 0;
        long segemntId = 0;
        long editionId = 0;
        string allAuthors = "";
        string authorDescription = "";
        long edition_id = 0;
        long serialNo = 0;
        string branch_name = "";
        public static string authorIds = "";
        public string authorNames = "";

        Publication publication = null;
        Segment segment = null;
        Edition edition = null;
        BookAuthor bookAuthor = null;


        // Database connection
        SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=LibraryMIS;Integrated Security=true");


        public ActionResult Index()
        {
            try
            {

                string sqlQuery = @"SELECT 
	                publication.s_no, 
	                publication.publication_name, 
	                publication.publication_type, 
	                publication.translator, 
	                publication.permission, 
	                publication.publication_language, 
	                branch.branch_name
	
                FROM 
	                Publication publication 
	                INNER JOIN Branch branch ON branch.branch_id = publication.branch_id	
                    WHERE publication.publication_type = 'Thesis'
	                ORDER BY publication.publication_name 
                ";

                // List to store query results
                List<BookResultModel> queryResults = new List<BookResultModel>();

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
                        BookResultModel result = new BookResultModel
                        {
                            // Map data from reader to result model properties
                            s_no = reader.GetInt64(reader.GetOrdinal("s_no")),
                            publication_name = reader.GetString(reader.GetOrdinal("publication_name")),
                            publication_type = reader.GetString(reader.GetOrdinal("publication_type")),
                            translator = reader.GetString(reader.GetOrdinal("translator")),
                            branch = reader.GetString(reader.GetOrdinal("branch_name")),
                            permission = reader.GetBoolean(reader.GetOrdinal("permission")),
                            publication_language = reader.GetString(reader.GetOrdinal("publication_language")),
                            authors = allAuthors
                        };

                        queryResults.Add(result);
                    }

                    reader.Close();
                }

                // Pass query results to view model
                var viewModel = new BookQueryResultViewModel
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

        public ActionResult ThesisRegistrationView()
        {
            List<Branch> branches = db.Branches.ToList();

            ViewBag.branch = branches;

            List<Author> authors = db.Authors.ToList();
            ViewBag.author = authors;

            return View();
        }

        [HttpPost]
        public JsonResult ThesisRegistration(Publication bookProperties, List<int> authors,Thesis thesis,MonographAndThesi monographAndThesisTable, List<EditionProperty> editionsProperty)
        {
            try
            {

                publication = new Publication();
                publication.ISBN = bookProperties.ISBN;
                if (bookProperties.ISBN == null || bookProperties.ISBN == "")
                    publication.ISBN = "خالي";
                publication.DDC_classificationNo = bookProperties.DDC_classificationNo;
                if (bookProperties.DDC_classificationNo == null || bookProperties.DDC_classificationNo == "")
                    publication.DDC_classificationNo = "خالي";
                publication.LLC_classificationNo = bookProperties.LLC_classificationNo;
                if (bookProperties.LLC_classificationNo == null || bookProperties.LLC_classificationNo == "")
                    publication.LLC_classificationNo = "خالي";
                publication.publication_type = "Thesis";
                publication.publication_name = bookProperties.publication_name;
                publication.translator = bookProperties.translator;
                publication.branch_id = bookProperties.branch_id;
                publication.permission = bookProperties.permission;
                publication.publication_language = bookProperties.publication_language;
                publication.publication_description = bookProperties.publication_description;
                if (bookProperties.publication_description == null || bookProperties.publication_description == "")
                    publication.publication_description = "خالي";


                db.Publications.Add(publication);
                db.SaveChanges();

                // Now get serial number of the book added
                long s_no = (long)db.Publications.OrderByDescending(col => col.s_no).FirstOrDefault().s_no;

                if (!(s_no > 0))
                {
                    return Json("publication not saved", JsonRequestBehavior.AllowGet);
                }
                else
                {

                    // Saving book authors
                    foreach (var author in authors)
                    {
                        bookAuthor = new BookAuthor();
                        bookAuthor.author_id = author;
                        bookAuthor.s_no = s_no;
                        db.BookAuthors.Add(bookAuthor);
                        db.SaveChanges();
                    }

                    // Saving Thesis table data
                    thesis.s_no = s_no;
                    db.Theses.Add(thesis);
                    db.SaveChanges();

                    // Saving MongraphAndThesis table data
                    monographAndThesisTable.s_no = s_no;
                    db.MonographAndThesis.Add(monographAndThesisTable);
                    db.SaveChanges();

                    // Saving each segment and edition of the book

                    foreach (var editionProp in editionsProperty)
                    {
                        if (editionProp.segmentNo != segemntNo)
                        {
                            // Saving book segment
                            segemntNo = editionProp.segmentNo;
                            segment = new Segment();
                            segment.s_no = s_no;
                            segment.segment_no = (short)segemntNo;
                            db.Segments.Add(segment);
                            db.SaveChanges();

                            // Now get segment number of the segment added
                            segemntId = (long)db.Segments.OrderByDescending(col => col.segment_id).FirstOrDefault().segment_id;

                        }

                        // Saving book edition

                        edition = new Edition();
                        edition.segment_id = segemntId;
                        edition.edition_no = (short)editionProp.editionNo;
                        edition.publication_quantity = (short)editionProp.publicationQuantity;
                        edition.publication_pages = (short)editionProp.publicationPages;
                        edition.cd_quantity = (short)editionProp.CDQuantity;
                        edition.registration_date_type = editionProp.registrationDateType;


                        string[] registrationDataParts = editionProp.registrationDate.Split('-');
                        edition.registration_year = int.Parse(registrationDataParts[0]);
                        edition.registration_month = int.Parse(registrationDataParts[1]);
                        edition.registration_day = int.Parse(registrationDataParts[2]);

                        edition.publication_date_type = editionProp.publicationDateType;
                        edition.publication_year = editionProp.publicationYear;
                        edition.publication_month = editionProp.publicationMonth;
                        edition.publication_day = editionProp.publicationDay;
                        edition.cupboard_no = (short)editionProp.cupboardNo;
                        edition.cell_no = (short)editionProp.cellNo;
                        db.Editions.Add(edition);
                        db.SaveChanges();

                        
                    }
                    Session["added"] = true;
                    var redirect = Url.Action("ThesisRegistrationView", "Thesis");
                    return Json(new
                    {
                        redirectTo = redirect
                    });

                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ThesisDeletion(int id)
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

        public ActionResult ThesisDetailsView(long id)
        {
            try
            {

                string sqlQuery = @"SELECT 
	                publication.*,
					branch.*,
					segment.*,
					edition.*,
					monographAndThesis.*,
					thesis.*
	
                FROM 
	                Publication publication 
	                INNER JOIN Branch branch ON branch.branch_id = publication.branch_id 
					INNER JOIN Segment segment ON segment.s_no = publication.s_no 
					INNER JOIN Edition edition ON edition.segment_id = segment.segment_id 
					INNER JOIN MonographAndThesis monographAndThesis ON monographAndThesis.s_no = publication.s_no 
					INNER JOIN Thesis thesis ON thesis.s_no = publication.s_no 
                    WHERE publication.s_no = " + id + " ORDER BY publication.publication_name";

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
                        allAuthors = "";
                        authorDescription = "";
                        edition_id = reader.GetInt64(reader.GetOrdinal("edition_id"));
                        serialNo = id;

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
                                    if (columnNo == 1)
                                    {
                                        authorIds += col + ",";

                                    }
                                    else if (columnNo == 2)
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
                                    if (columnNo == 1)
                                    {
                                        authorIds += col + ",";

                                    }
                                    else if (columnNo == 2)
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




                        int availableQuantity = 0;

                        availableQuantity = db.Dealings.Where(d => d.edition_id == edition_id && d.returned == false).Count();
                        int totalQuantity = Convert.ToInt32(reader.GetInt16(reader.GetOrdinal("publication_quantity")));
                        availableQuantity = totalQuantity - availableQuantity;

                        branch_name = reader.GetString(reader.GetOrdinal("branch_name"));

                        // Map data from reader to result model
                        PublicationResultModel result = new PublicationResultModel
                        {

                            // Map data from reader to result model properties
                            s_no = reader.GetInt64(reader.GetOrdinal("s_no")),
                            ISBN = reader.GetString(reader.GetOrdinal("ISBN")),
                            DDC_classificationNo = reader.GetString(reader.GetOrdinal("DDC_classificationNo")),
                            LLC_classificationNo = reader.GetString(reader.GetOrdinal("LLC_classificationNo")),
                            publication_description = reader.GetString(reader.GetOrdinal("publication_description")),
                            publication_name = reader.GetString(reader.GetOrdinal("publication_name")),
                            publication_type = reader.GetString(reader.GetOrdinal("publication_type")),
                            translator = reader.GetString(reader.GetOrdinal("translator")),
                            branch = reader.GetString(reader.GetOrdinal("branch_name")),
                            permission = reader.GetBoolean(reader.GetOrdinal("permission")),
                            publication_language = reader.GetString(reader.GetOrdinal("publication_language")),
                            authors = allAuthors,
                            author_description = authorDescription,
                            segment_id = reader.GetInt64(reader.GetOrdinal("segment_id")),
                            segment_no = reader.GetInt16(reader.GetOrdinal("segment_no")),
                            edition_id = reader.GetInt64(reader.GetOrdinal("edition_id")),
                            edition_no = reader.GetInt16(reader.GetOrdinal("edition_no")),
                            publication_quantity = reader.GetInt16(reader.GetOrdinal("publication_quantity")),
                            availableQuantity = availableQuantity,
                            publication_pages = reader.GetInt16(reader.GetOrdinal("publication_pages")),
                            cd_quantity = reader.GetInt16(reader.GetOrdinal("cd_quantity")),
                            registration_date_type = reader.GetString(reader.GetOrdinal("registration_date_type")),
                            registration_year = reader.GetInt32(reader.GetOrdinal("registration_year")),
                            registration_month = reader.GetInt32(reader.GetOrdinal("registration_month")),
                            registration_day = reader.GetInt32(reader.GetOrdinal("registration_day")),
                            publication_date_type = reader.GetString(reader.GetOrdinal("publication_date_type")),
                            publication_year = reader.GetInt32(reader.GetOrdinal("publication_year")),
                            publication_month = reader.GetInt32(reader.GetOrdinal("publication_month")),
                            publication_day = reader.GetInt32(reader.GetOrdinal("publication_day")),
                            cupboard_no = reader.GetInt16(reader.GetOrdinal("cupboard_no")),
                            cell_no = reader.GetInt16(reader.GetOrdinal("cell_no")),
                            /*publication_no = reader.GetInt16(reader.GetOrdinal("publication_no")),
                            publication_place = reader.GetString(reader.GetOrdinal("publication_place")),
                            publisher_name = reader.GetString(reader.GetOrdinal("publisher_name")),*/
                            thesis_id = reader.GetInt64(reader.GetOrdinal("thesis_id")),
                            supervisor_firstname = reader.GetString(reader.GetOrdinal("supervisor_firstname")),
                            supervisor_lastname = reader.GetString(reader.GetOrdinal("supervisor_lastname")),
                            student_registration_year = reader.GetString(reader.GetOrdinal("student_registration_year")),
                            student_graduation_year = reader.GetString(reader.GetOrdinal("student_graduation_year")),
                            defence_data = reader.GetDateTime(reader.GetOrdinal("defence_data")),
                            mark = reader.GetString(reader.GetOrdinal("mark")),
                            graduation_period = reader.GetString(reader.GetOrdinal("graduation_period")),
                            monograph_thesis_id = reader.GetInt64(reader.GetOrdinal("monograph_thesis_id")),
                            internal_conflict = reader.GetString(reader.GetOrdinal("internal_conflict")),
                            external_conflict = reader.GetString(reader.GetOrdinal("external_conflict"))
                        };

                        queryResults.Add(result);
                    }

                    reader.Close();
                }

                List<Branch> branches = db.Branches.ToList();

                ViewBag.branch = branches;

                List<Author> authors = db.Authors.ToList();
                ViewBag.author = authors;

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

        public ActionResult ThesisEditView(long id)
        {
            authorIds = "";
            try
            {

                string sqlQuery = @"SELECT 
	                publication.*,
					branch.*,
					segment.*,
					edition.*,
					monographAndThesis.*,
					thesis.*
	
                FROM 
	                Publication publication 
	                INNER JOIN Branch branch ON branch.branch_id = publication.branch_id 
					INNER JOIN Segment segment ON segment.s_no = publication.s_no 
					INNER JOIN Edition edition ON edition.segment_id = segment.segment_id 
					INNER JOIN MonographAndThesis monographAndThesis ON monographAndThesis.s_no = publication.s_no 
					INNER JOIN Thesis thesis ON thesis.s_no = publication.s_no 
                    WHERE publication.s_no = " + id + " ORDER BY publication.publication_name";

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
                        allAuthors = "";
                        authorDescription = "";
                        edition_id = reader.GetInt64(reader.GetOrdinal("edition_id"));
                        serialNo = id;

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
                                    if (columnNo == 1)
                                    {
                                        authorIds += col + ",";

                                    }
                                    else if (columnNo == 2)
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
                                    if (columnNo == 1)
                                    {
                                        authorIds += col + ",";

                                    }
                                    else if (columnNo == 2)
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




                        int availableQuantity = 0;

                        availableQuantity = db.Dealings.Where(d => d.edition_id == edition_id && d.returned == false).Count();
                        int totalQuantity = Convert.ToInt32(reader.GetInt16(reader.GetOrdinal("publication_quantity")));
                        availableQuantity = totalQuantity - availableQuantity;

                        branch_name = reader.GetString(reader.GetOrdinal("branch_name"));

                        // Map data from reader to result model
                        PublicationResultModel result = new PublicationResultModel
                        {

                            // Map data from reader to result model properties
                            s_no = reader.GetInt64(reader.GetOrdinal("s_no")),
                            ISBN = reader.GetString(reader.GetOrdinal("ISBN")),
                            DDC_classificationNo = reader.GetString(reader.GetOrdinal("DDC_classificationNo")),
                            LLC_classificationNo = reader.GetString(reader.GetOrdinal("LLC_classificationNo")),
                            publication_description = reader.GetString(reader.GetOrdinal("publication_description")),
                            publication_name = reader.GetString(reader.GetOrdinal("publication_name")),
                            publication_type = reader.GetString(reader.GetOrdinal("publication_type")),
                            translator = reader.GetString(reader.GetOrdinal("translator")),
                            branch = reader.GetString(reader.GetOrdinal("branch_name")),
                            permission = reader.GetBoolean(reader.GetOrdinal("permission")),
                            publication_language = reader.GetString(reader.GetOrdinal("publication_language")),
                            authors = allAuthors,
                            author_description = authorDescription,
                            segment_id = reader.GetInt64(reader.GetOrdinal("segment_id")),
                            segment_no = reader.GetInt16(reader.GetOrdinal("segment_no")),
                            edition_id = reader.GetInt64(reader.GetOrdinal("edition_id")),
                            edition_no = reader.GetInt16(reader.GetOrdinal("edition_no")),
                            publication_quantity = reader.GetInt16(reader.GetOrdinal("publication_quantity")),
                            availableQuantity = availableQuantity,
                            publication_pages = reader.GetInt16(reader.GetOrdinal("publication_pages")),
                            cd_quantity = reader.GetInt16(reader.GetOrdinal("cd_quantity")),
                            registration_date_type = reader.GetString(reader.GetOrdinal("registration_date_type")),
                            registration_year = reader.GetInt32(reader.GetOrdinal("registration_year")),
                            registration_month = reader.GetInt32(reader.GetOrdinal("registration_month")),
                            registration_day = reader.GetInt32(reader.GetOrdinal("registration_day")),
                            publication_date_type = reader.GetString(reader.GetOrdinal("publication_date_type")),
                            publication_year = reader.GetInt32(reader.GetOrdinal("publication_year")),
                            publication_month = reader.GetInt32(reader.GetOrdinal("publication_month")),
                            publication_day = reader.GetInt32(reader.GetOrdinal("publication_day")),
                            cupboard_no = reader.GetInt16(reader.GetOrdinal("cupboard_no")),
                            cell_no = reader.GetInt16(reader.GetOrdinal("cell_no")),
                            /*publication_no = reader.GetInt16(reader.GetOrdinal("publication_no")),
                            publication_place = reader.GetString(reader.GetOrdinal("publication_place")),
                            publisher_name = reader.GetString(reader.GetOrdinal("publisher_name")),*/
                            thesis_id = reader.GetInt64(reader.GetOrdinal("thesis_id")),
                            monograph_thesis_id = reader.GetInt64(reader.GetOrdinal("monograph_thesis_id")),
                            supervisor_firstname = reader.GetString(reader.GetOrdinal("supervisor_firstname")),
                            supervisor_lastname = reader.GetString(reader.GetOrdinal("supervisor_lastname")),
                            student_registration_year = reader.GetString(reader.GetOrdinal("student_registration_year")),
                            student_graduation_year = reader.GetString(reader.GetOrdinal("student_graduation_year")),
                            defence_data = reader.GetDateTime(reader.GetOrdinal("defence_data")),
                            mark = reader.GetString(reader.GetOrdinal("mark")),
                            graduation_period = reader.GetString(reader.GetOrdinal("graduation_period")),
                            internal_conflict = reader.GetString(reader.GetOrdinal("internal_conflict")),
                            external_conflict = reader.GetString(reader.GetOrdinal("external_conflict"))
                        };

                        queryResults.Add(result);
                    }

                    reader.Close();
                }

                List<Branch> branches = db.Branches.ToList();

                ViewBag.branch = branches;

                List<Author> authors = db.Authors.ToList();
                ViewBag.author = authors;

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
        public JsonResult EditEditionAndSegment( Segment segment, Edition edition, int currentSegmentNo, int currentEditionNo)
        {

            try
            {
                int previousSegmentNo = db.Segments.Where(s => s.segment_id == segment.segment_id).Select(c => c.segment_no).FirstOrDefault();

                if (previousSegmentNo == currentSegmentNo)
                {
                    db.Entry(segment).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    int doesSegmentExists = db.Segments.Where(s => s.segment_id == segment.segment_id && s.segment_no == segment.segment_no).Count();
                    if (doesSegmentExists > 0)
                    {
                        return Json("segment exists", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        db.Entry(segment).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                int previousEditionNo = db.Editions.Where(e => e.edition_id == edition.edition_id).Select(c => c.edition_no).FirstOrDefault();

                if (previousEditionNo == currentEditionNo)
                {
                    db.Entry(edition).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    int doesEditionExists = db.Segments.Where(s => s.segment_id == segment.segment_id && s.segment_no == segment.segment_no).Count();
                    if (doesEditionExists > 0)
                    {
                        return Json("edition exists", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        db.Entry(edition).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                

                return Json("updated", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);

            }

        }

        [HttpPost]
        public JsonResult EditPublication(Publication bookProperties,MonographAndThesi monographAndThesis, Thesis thesis, List<int> authors)
        {
            try
            {
                int permission;
                if (bookProperties.permission == true)
                {
                    permission = 1;

                }
                else
                {
                    permission = 0;
                }
                // Open connection
                con.Open();
                // Update information
                string query1 = "UPDATE [Publication] SET [ISBN] = N'" + bookProperties.ISBN + "', " +
                    "[DDC_classificationNo] = N'" + bookProperties.DDC_classificationNo + "', " +
                    "[LLC_classificationNo] = N'" + bookProperties.LLC_classificationNo + "', " +
                    "[publication_name] = N'" + bookProperties.publication_name + "', " +
                    "[translator] = N'" + bookProperties.translator + "', " +
                    "[branch_id] = " + bookProperties.branch_id + ", " +
                    "[permission] = " + permission + ", " +
                    "[publication_language] = N'" + bookProperties.publication_language + "', " +
                    "[publication_description] = N'" + bookProperties.publication_description + "'" +
                    " WHERE s_no = " + bookProperties.s_no;

                SqlCommand cmd1 = new SqlCommand(query1, con);
                cmd1.ExecuteNonQuery();
                //Close connection
                con.Close();

                db.SaveChanges();

                // Open connection
                con.Open();
                // Update information
                string query4 = "UPDATE [MonographAndThesis] " +
                   "SET [supervisor_firstname] = N'" + monographAndThesis.supervisor_firstname + "', " +
                   "[supervisor_lastname] = N'" + monographAndThesis.supervisor_lastname + "', " +
                   "[student_graduation_year] = N'" + monographAndThesis.student_graduation_year + "', " +
                   "[student_registration_year] = N'" + monographAndThesis.student_registration_year + "', " +
                   "[defence_data] = '" + Convert.ToDateTime(monographAndThesis.defence_data) + "', " +
                   "[mark] = N'" + monographAndThesis.mark + "', " +
                   "[graduation_period] =  N'" + monographAndThesis.graduation_period+ "' " +
                   "WHERE monograph_thesis_id = " + monographAndThesis.monograph_thesis_id;

                SqlCommand cmd4 = new SqlCommand(query4, con);
                cmd4.ExecuteNonQuery();
                //Close connection
                con.Close();
                db.SaveChanges();

                // Open connection
                con.Open();
                // Update information
                string query5 = "UPDATE [Thesis] " +
                   "SET [internal_conflict] = N'" + thesis.internal_conflict+ "', " +
                   "[external_conflict] = N'" + thesis.external_conflict + "' " +
                   "WHERE [thesis_id] = " + thesis.thesis_id;

                SqlCommand cmd5 = new SqlCommand(query5, con);
                cmd5.ExecuteNonQuery();
                //Close connection
                con.Close();
                db.SaveChanges();

                long serialNo = bookProperties.s_no;

                // Open connection
                con.Open();
                // Delete authors
                string query2 = "DELETE FROM BookAuthor WHERE s_no = " + bookProperties.s_no + "";
                SqlCommand cmd2 = new SqlCommand(query2, con);
                cmd2.ExecuteNonQuery();
                //Close connection
                con.Close();

                db.SaveChanges();

                // Add book authors
                foreach (var author in authors)
                {
                    bookAuthor = new BookAuthor();
                    bookAuthor.author_id = author;
                    bookAuthor.s_no = serialNo;
                    db.BookAuthors.Add(bookAuthor);
                    db.SaveChanges();
                }
                
                var redirectTo = Url.Action("Index", "Thesis");
                return Json(new
                {
                    redirectTo = redirectTo
                });

            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);

            }

        }

        [HttpPost]
        public JsonResult AddSegment(EditionProperties editionsProperty)
        {
            try
            {


                // Saving segment of the book

                var segment = db.Segments.Where(s => s.s_no == editionsProperty.s_no && s.segment_no == editionsProperty.segmentNo).FirstOrDefault();
                if (segment != null)
                {

                    // Saving book segment
                    segemntNo = editionsProperty.segmentNo;
                    segment = new Segment();
                    segment.s_no = editionsProperty.s_no;
                    segment.segment_no = (short)segemntNo;
                    db.Segments.Add(segment);
                    db.SaveChanges();

                    // Now get id of the segment added
                    segemntId = (long)db.Segments.OrderByDescending(col => col.segment_id).FirstOrDefault().segment_id;
                }
                else
                {
                    segemntId = (long)db.Segments.Where(s => s.segment_no == editionsProperty.segmentNo && s.s_no == editionsProperty.s_no).FirstOrDefault().segment_id;

                }


                // Saving book edition

                edition = new Edition();
                edition.segment_id = segemntId;
                edition.edition_no = (short)editionsProperty.editionNo;
                edition.publication_quantity = (short)editionsProperty.publicationQuantity;
                edition.publication_pages = (short)editionsProperty.publicationPages;
                edition.cd_quantity = (short)editionsProperty.CDQuantity;
                edition.registration_date_type = editionsProperty.registrationDateType;


                string[] registrationDataParts = editionsProperty.registrationDate.Split('-');
                edition.registration_year = int.Parse(registrationDataParts[0]);
                edition.registration_month = int.Parse(registrationDataParts[1]);
                edition.registration_day = int.Parse(registrationDataParts[2]);

                edition.publication_date_type = editionsProperty.publicationDateType;
                edition.publication_year = editionsProperty.publicationYear;
                edition.publication_month = editionsProperty.publicationMonth;
                edition.publication_day = editionsProperty.publicationDay;
                edition.cupboard_no = (short)editionsProperty.cupboardNo;
                edition.cell_no = (short)editionsProperty.cellNo;
                db.Editions.Add(edition);
                db.SaveChanges();

                // Now get edition id to fill bookAndMagazine table 
                editionId = (long)db.Editions.OrderByDescending(col => col.edition_id).FirstOrDefault().edition_id;

                var redirect = Url.Action("Index", "Thesis");
                return Json(new
                {
                    redirectTo = redirect
                });


            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult DeleteEdition(long s_no, long segmentId, long editionId)
        {
            try
            {
                int totalSegmentsOfPublications = db.Segments.Where(s => s.s_no == s_no).Count();
                if (totalSegmentsOfPublications == 1)
                {
                    Publication publication = db.Publications.Where(p => p.s_no == s_no).FirstOrDefault();
                    if (db.Publications.Remove(publication) != null)
                    {
                        db.SaveChanges();
                        var redirectTo = Url.Action("Index", "Thesis");
                        return Json(new
                        {
                            redirectTo = redirectTo
                        });
                    }
                    else
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);

                    }

                }
                else
                {

                    int totalEditionOfSegment = db.Editions.Where(e => e.segment_id == segmentId).Count();
                    if (totalEditionOfSegment > 1)
                    {
                        Edition edition = db.Editions.Where(e => e.edition_id == editionId).FirstOrDefault();
                        if (db.Editions.Remove(edition) != null)
                        {
                            db.SaveChanges(); var redirectTo = Url.Action("Index", "Thesis");
                            return Json(new
                            {
                                redirectTo = redirectTo
                            });

                        }
                        else
                        {
                            return Json(false, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        Segment segment = db.Segments.Where(s => s.segment_id == segmentId).FirstOrDefault();
                        if (db.Segments.Remove(segment) != null)
                        {
                            db.SaveChanges(); var redirectTo = Url.Action("Index", "Thesis");
                            return Json(new
                            {
                                redirectTo = redirectTo
                            });
                        }
                        else
                        {
                            return Json(false, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }


        }







        public class EditionProperty
        {

            public int No { get; set; }
            public string bookName { get; set; }
            public int segmentNo { get; set; }
            public int editionNo { get; set; }
            public int publicationQuantity { get; set; }
            public int publicationPages { get; set; }
            public int CDQuantity { get; set; }
            public string registrationDateType { get; set; }
            public string registrationDate { get; set; }
            public string publicationDateType { get; set; }
            public int publicationYear { get; set; }
            public int publicationMonth { get; set; }
            public int publicationDay { get; set; }
            public int cupboardNo { get; set; }
            public int cellNo { get; set; }

        }
    }



}