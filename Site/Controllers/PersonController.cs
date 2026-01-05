using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Site.Models;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
namespace Site.Controllers
{
    public class PersonController : Controller
    {
        // GET: Person
        LibraryMISEntities db = new LibraryMISEntities();
        SqlConnection con = new SqlConnection("Data Source=DESKTOP-LABF8GR\\SQLEXPRESS; Initial Catalog=LibraryMIS; Integrated Security = true");

        public ActionResult Index()
        {
            var person = db.People;
            return View(person);
        }

        public JsonResult PersonCheck(int id)
        {
            Person data = db.People.Find(id);
            var obj = new
            {
                person_name = data.person_firstname,
                person_father_name = data.person_father_name
            };


            return Json(obj, JsonRequestBehavior.AllowGet);
        }


        public ActionResult TeacherRegistrationView()
        {
            return View();
        }

        [HttpPost]
        public JsonResult TeacherRegistration(Person person)
        {
            try
            {
                db.People.Add(person);
                var res = db.SaveChanges();
                if (res > 0)
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
                throw;
            }

        }

        public ActionResult TeacherView()
        {
            var data = db.People.Where(p => p.person_state == "teacher");
            return View(data);
        }

        public JsonResult TeacherDelete(int id)
        {
            try
            {
                var data = db.People.Find(id);
                db.People.Remove(data);
                var res = db.SaveChanges();
                if (res > 0)
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

        public ActionResult TeacherEditView(int id)
        {
            var data = db.People.Find(id);
            return View(data);
        }

        public JsonResult TeacherEdit(Person person)
        {
            try
            {
                Person data = db.People.Find(person.person_id);
                data.person_idCard = person.person_idCard;
                data.person_firstname = person.person_firstname;
                data.person_lastname = person.person_lastname;
                data.person_father_name = person.person_father_name;

                data.gender = person.gender;
                data.university = person.university;
                data.faculty = person.faculty;
                data.department = person.department;
                data.phone_number = person.phone_number;
                data.email = person.email;
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult StudentRegistrationView()
        {
            return View();
        }

        public JsonResult StudentRegistration(Person personDetails, Student studentDetails)
        {
            try
            {
                Person person = new Person();
                person.person_idCard = personDetails.person_idCard;
                person.person_firstname = personDetails.person_firstname;
                person.person_lastname = personDetails.person_lastname;
                person.person_father_name = personDetails.person_father_name;
                person.person_state = "student";
                person.gender = personDetails.gender;
                person.university = personDetails.university;
                person.faculty = personDetails.faculty;
                person.department = personDetails.department;
                person.phone_number = personDetails.phone_number;
                person.email = personDetails.email;
                db.People.Add(person);
                db.SaveChanges();
                Student student = new Student();

                long person_id = db.People.OrderByDescending(o => o.person_id).FirstOrDefault().person_id;
                student.person_id = person_id;
                student.semester = studentDetails.semester;
                db.Students.Add(student);
                var res = db.SaveChanges();
                if (res > 0)
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

        public ActionResult StudentView()
        {

            var data = (from t1 in db.People
                        join t2 in db.Students on t1.person_id equals t2.person_id
                        select new
                        combinedPersonStudentViewModel
                        {
                            person_id = t1.person_id,
                            person_idCard = t1.person_idCard,
                            person_firstname = t1.person_firstname,
                            person_lastname = t1.person_lastname,
                            person_father_name = t1.person_father_name,

                            gender = t1.gender,
                            university = t1.university,
                            faculty = t1.faculty,
                            department = t1.department,
                            semester = t2.semester,
                            phone_number = t1.phone_number,
                            email = t1.email,
                        }).ToList();

            return View(data);
        }

        public JsonResult StudentDelete(long id)
        {
            try
            {

                Person person = db.People.Find(id);
                db.People.Remove(person);
                var res = db.SaveChanges();
                if (res > 0)
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


        public ActionResult StudentEditView(long id)
        {

            var data = (from p in db.People
                        join s in db.Students on p.person_id equals s.person_id
                        where p.person_id == id
                        select new
                        combinedPersonStudentViewModel
                        {


                            person_id = p.person_id,
                            person_idCard = p.person_idCard,
                            person_firstname = p.person_firstname,
                            person_lastname = p.person_lastname,
                            person_father_name = p.person_father_name,

                            gender = p.gender,
                            university = p.university,
                            faculty = p.faculty,
                            department = p.department,
                            semester = s.semester,
                            phone_number = p.phone_number,
                            email = p.email
                        }).FirstOrDefault();

            return View(data);



        }

        public JsonResult StudentEdit(Person personData, Student studentData)
        {
            try
            {
                Person person = db.People.Find(personData.person_id);
                person.person_id = personData.person_id;
                person.person_firstname = personData.person_firstname;
                person.person_lastname = personData.person_lastname;
                person.person_father_name = personData.person_father_name;
                person.person_state = "student";
                person.gender = personData.gender;
                person.university = personData.university;
                person.faculty = personData.faculty;
                person.department = personData.department;
                person.phone_number = personData.phone_number;
                person.email = personData.email;

                db.SaveChanges();

                con.Open();
                string query = "UPDATE [Student] SET [semester] = N'" + studentData.semester + "' WHERE [person_id] =" + personData.person_id;

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                con.Close();

                return Json(true, JsonRequestBehavior.AllowGet);




            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
                
            }
        }

        public ActionResult EmployeeRegistrationView()
        {
            return View();
        }

        public JsonResult EmployeeRegistration(Person person)
        {
            try
            {
                db.People.Add(person);
                int res = db.SaveChanges();
                if (res > 0)
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

        public ActionResult EmployeeView()
        {
            var data = db.People.Where(s => s.department == "" || s.department == null);
            return View(data);
        }

        public JsonResult EmployeeDelete(long id)
        {
            try
            {
                Person person = db.People.Find(id);
                db.People.Remove(person);
                int res = db.SaveChanges();
                if (res > 0)
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

        public ActionResult EmployeeEditView(long id)
        {

            var data = db.People.Find(id);
            return View(data);

        }

        public JsonResult EmployeeEdit(Person person)
        {
            try
            {
                Person data = db.People.Find(person.person_id);
                data.person_idCard = person.person_idCard;
                data.person_firstname = person.person_firstname;
                data.person_lastname = person.person_lastname;
                data.person_father_name = person.person_father_name;
                data.person_state = person.person_state;
                data.gender = person.gender;
                data.university = person.university;
                data.faculty = person.faculty;
                data.department = "";
                data.phone_number = person.phone_number;
                data.email = person.email;
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);

            }
        }
    }






}