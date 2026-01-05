using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Site.ViewModel;
using Site.Models;

namespace Site.Controllers
{
    
    public class HomeController : Controller
    {
        LibraryMISEntities db = new LibraryMISEntities();
        public ActionResult Index()
        {
            
            return View();
        }
        
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Login(string username, string password, string email, string level)
        {
            try
            {
                var file = Request.Files["file"];

                User user = new User();
                user.username = username;
                user.password = password;
                user.email = email;
                user.level = level;

                if (file != null && file.ContentLength > 0)
                {
                    string fileName = Guid.NewGuid() + Path.GetFileName(file.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/assets/pic/"), fileName);
                    file.SaveAs(filePath);

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
                    return Json("New user registered successfully", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Something went wrong", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                // Handle error
                return Json("Error uploading file: " + ex.Message);
            }
        }
      
    }
}
