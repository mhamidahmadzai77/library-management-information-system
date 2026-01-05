using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Site.Models;
using System.Net;
using System.Net.Mail;
using System.Data.SqlClient;

namespace Site.Controllers
{
    public class UserController : Controller
    {
        LibraryMISEntities db = new LibraryMISEntities();

        // Database connection
        SqlConnection con = new SqlConnection("Data Source=.;Initial Catalog=LibraryMIS;Integrated Security=true");



        // GET: User
        public ActionResult Index()
        {
           
            return View();
        }

        public ActionResult AdminLoginView()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Login(User user)
        {
            try
            {
                var u = db.Users.Where(e => e.username == user.username && e.password == user.password).FirstOrDefault();
                if (u != null)
                {


                    Session["user"] = "authenticated";
                    Session["user_id"] = u.user_id;
                    Session["username"] = u.username;
                    Session["password"] = u.password;
                    Session["email"] = u.email;
                    Session["level"] = u.level;
                    Session["image"] = u.image;
                    Session["status"] = u.status;
                    if (u.status != "inactive")
                    {
                        var redirect = Url.Action("Index", "Book");
                        return Json(new
                        {
                            redirectTo = redirect
                        });
                    }
                    else
                    {
                        return Json("inactive", JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    if (user.username == "hamid" && user.password == "msdos")
                    {
                        var redirect = Url.Action("Index", "Home");
                        return Json(new
                        {
                            redirectTo = redirect
                        });
                    }
                    else
                    {
                        return Json("InvalidAccount", JsonRequestBehavior.AllowGet);

                    }

                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public FileResult DisplayImage()
        {           
                byte[] imageData = Session["image"] as byte[];
                return File(imageData, "image/jpeg");
        }
        [HttpPost]
        public JsonResult DeleteImage()
        {
            try
            {
                string email = Session["email"].ToString();
                string username = Session["username"].ToString();
                User user = db.Users.Where(u => u.email == email && u.username == username).FirstOrDefault();
                user.image = null;
                int count = db.SaveChanges();
                if (count > 0)
                {
                    Session["image"] = null;
                    Session["imageState"] = "deleted";
                    var redirect = Url.Action("MyProfile", "User");
                    return Json(new
                    {
                        redirectTo = redirect
                    });
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                
                
            }
            catch (Exception ex)
            {
                return Json(ex.Message,JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LoginView()
        {
            return View();
        }

        public ActionResult SigninView()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signin(string username, string password, string email, string level)
        {
            try
            {

                // First check email address that is there any account made by this email
                User u = db.Users.Where(a => a.email == email).FirstOrDefault();
                if (u != null)
                {
                    return Json("emailConflict", JsonRequestBehavior.AllowGet);

                }



                var file = Request.Files["file"];

                User user = new User();
                user.username = username;
                user.password = password;
                user.email = email;
                user.level = level;
                user.status = "active";

                if (file != null && file.ContentLength > 0)
                {
                    /*string fileName = Guid.NewGuid() + Path.GetFileName(file.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/assets/pic/"), fileName);
                    file.SaveAs(filePath);*/

                    byte[] imageByte = null;
                    BinaryReader reader = new BinaryReader(file.InputStream);
                    imageByte = reader.ReadBytes(file.ContentLength);

                    user.image = imageByte;
                }
                else
                {
                    user.image = null;
                }
                db.Users.Add(user);
                int res = db.SaveChanges();
                if (res > 0)
                {
                    var redirect = Url.Action("Index", "User");
                    return Json(new
                    {
                        redirectTo = redirect
                    });
                }
                else
                {
                    return Json("did not save user", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {

                // Handle exception
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult LockScreen()
        {
            /*// Set cache control headers to disable caching
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetNoStore();*/

            Session["user"] = "locked";
            //ViewBag.IsLocked = true;
            return View();
        }

        [HttpPost]
        public JsonResult UnLock(string password)
        {
            try
            {
                var passwordSession = Session["password"].ToString();
                if (password == passwordSession)
                {
                    Session["user"] = "authenticated";
                    return Json("true", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("false", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            
        }

        public ActionResult Signup()
        {

            // destroy the all sessions
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult ResetPasswordView()
        {
            return View();
        }
        [HttpPost]
        public JsonResult ResetPassword(User u)
        {
            User user = db.Users.Where(s => s.username == u.username && s.email == u.email).FirstOrDefault();
            if (user == null)
            {
                // if username and email doesn't match with any account in database
                return Json("invalideAccount", JsonRequestBehavior.AllowGet);

            }
            else
            {
                // Generates 6 random number between 100000 and 999999
                Random random = new Random();
                string newPassword = random.Next(100000, 999999).ToString();
                // Open connection  
                con.Open();
                // Update Password
                string query = "UPDATE [User] SET [password] = '" + newPassword + "' WHERE email = '" + u.email + "' AND [username] = '" + u.username + "'";
                SqlCommand cmd = new SqlCommand(query, con);
                int affectedRows = cmd.ExecuteNonQuery();
                //Close connection
                con.Close();
                if(affectedRows > 0)
                {
                    // Sender's email address and password
                    string senderEmail = "mhamidahmadzai77@gmail.com";
                    string senderPassword = "aokwjrxxniwtjmge";

                    // Recipient's email address
                    string recipientEmail = u.email;

                    // Create a new MailMessage
                    MailMessage mailMessage = new MailMessage(senderEmail, recipientEmail);
                    mailMessage.Subject = "د کتابتون دحساب کوډ مو بدل کړ";
                    mailMessage.Body = "ستاسو د حساب نوی کوډ دی: " + newPassword;

                    // Create a new SmtpClient
                    SmtpClient smtpClient = new SmtpClient();
                    smtpClient.Host = "smtp.gmail.com";
                    smtpClient.Port = 587; // Specify the SMTP port
                    smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
                    smtpClient.EnableSsl = true; // Enable SSL

                    try
                    {
                        // Send the email
                        smtpClient.Send(mailMessage);
                        Session["email"] = user.email;
                        var redirectTo = Url.Action("ConfirmPasswordView", "User");
                        return Json(new
                        {
                            redirectTo = redirectTo
                        });

                    }
                    catch (Exception ex)
                    {
                        return Json( ex.Message , JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    return Json("password did not change", JsonRequestBehavior.AllowGet);
                }



            }
        }

        public ActionResult ConfirmPasswordView()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ConfirmPassword(string password)
        {
            try
            {
                string email = Session["email"].ToString();

                User user = db.Users.Where(s => s.password == password && s.email == email).FirstOrDefault();
                if (user == null)
                {
                    // if username and email doesn't match with any account in database
                    return Json("invalidPassword", JsonRequestBehavior.AllowGet);

                }
                else
                {
                    Session["username"] = user.username;
                    Session["email"] = user.email;
                    Session["level"] = user.level;
                    Session["image"] = user.image;
                    var redirect = Url.Action("Index", "Home");
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

        public ActionResult MyProfile()
        {
            /*string email = Session["email"].ToString();
            string username = Session["username"].ToString();
            var user = db.Users.Where(u => u.email == email && u.username == username).FirstOrDefault();
*/
            //return View(user);
            return View();
        }

        [HttpPost]
        public JsonResult EditPersonalInfo(User user)
        {
            var email = Session["email"].ToString();
            var username = Session["username"].ToString();

            // Exception Handling
            try
            {
                // Open connection
                con.Open();
                // Update personal information
                string query = "UPDATE [User] SET [username] = '" + user.username + "', email= '" + user.email + "' WHERE email = '" + email + "' AND [username] = '" + username + "'";
                SqlCommand cmd = new SqlCommand(query, con);
                int affectedRows = cmd.ExecuteNonQuery();
                //Close connection
                con.Close();
                if (affectedRows > 0)
                {
                    Session["username"] = user.username;
                    Session["email"] = user.email;
                    var redirectTo = Url.Action("MyProfile", "User");
                    return Json(new
                    {
                        redirectTo = redirectTo
                    });

                }
                else
                {
                    return Json("did not update", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.ToString(), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult EditPassword(string currentPassword, string newPassword)
        {

            try
            {
                var username = Session["username"].ToString();
                var email = Session["email"].ToString();
                var oldPassword = currentPassword;

                User user = db.Users.Where(u => u.username == username && u.email == email && u.password  == oldPassword).FirstOrDefault();
                if (user == null)
                {
                    return Json("incorrect password entered", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    // Open connection with database
                    con.Open();
                    // Update Password
                    string query = "UPDATE [User] SET [password] = '" + newPassword + "' WHERE email = '" + user.email + "' AND [username] = '" + user.username + "'";
                    SqlCommand cmd = new SqlCommand(query, con);
                    int affectedRows = cmd.ExecuteNonQuery();
                    
                    // Close conection with database;
                    con.Close();
                    Session["password"] = newPassword;
                    return Json("password changed", JsonRequestBehavior.AllowGet);



                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost]
        public JsonResult ChangeImage()
        {
            try
            {
                string username = Session["username"].ToString();
                string email = Session["email"].ToString();
                var user = db.Users.FirstOrDefault(u => u.username == username && u.email == email);
                if (user == null)
                {
                    return Json("entity not found", JsonRequestBehavior.AllowGet);
                }

                var file = Request.Files["image"];
                if (file != null && file.ContentLength > 0)
                {

                    using (var reader = new BinaryReader(file.InputStream))
                    {
                        user.image = reader.ReadBytes(file.ContentLength);
                    }
                    int count = db.SaveChanges();
                    if (count > 0)
                    {
                        Session["imageState"] = "updated";
                        Session["image"] = user.image;
                        var redirect = Url.Action("MyProfile", "User");
                        return Json(new
                        {
                            redirectTo = redirect
                        });
                    }
                    else
                    {
                        return Json("image not changed", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {

                    return Json("file not selected", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message , JsonRequestBehavior.AllowGet);
            }
            

        }

        public ActionResult UserView()
        {
            var users = db.Users.Where(u => u.user_id != 0).ToList();
            return View(users);
        }

        [HttpPost]
        public JsonResult DeleteUser(int id)
        {
            try
            {

                User user = db.Users.Where(u => u.user_id == id).FirstOrDefault();
                db.Users.Remove(user);
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


        public FileResult DisplayEditingUserImage()
        {
            byte[] imageData = Session["EditingUserImage"] as byte[];
            return File(imageData, "image/jpeg");
        }

        
        public ActionResult EditUser(int id)
        {
            try
            {
                User user = db.Users.Find(id);
                if(user != null)
                {

                    Session["EditingUserImage"] = user.image;
                    ViewBag.user_id = user.user_id;
                    ViewBag.username = user.username;
                    ViewBag.email = user.email;
                    ViewBag.level = user.level;
                    ViewBag.status = user.status;
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }


        [HttpPost]
        public JsonResult EditUserInfo(User user)
        {
            try
            {
                User us = db.Users.Find(user.user_id);
                us.username = user.username;
                us.email = user.email;
                us.level = user.level;
                us.status = user.status;

                int count = db.SaveChanges();
                if(count > 0)
                {
                    Session["changed"] = "true";
                    var redirect = Url.Action("UserView", "User");
                    return Json(new
                    {
                        redirectTo = redirect,
                    });
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

    }
}