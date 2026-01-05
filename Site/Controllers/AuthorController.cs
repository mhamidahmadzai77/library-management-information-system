using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Site.Models;

namespace Site.Controllers
{
    public class AuthorController : Controller
    {
        
        LibraryMISEntities db = new LibraryMISEntities();
        // GET: Author
        public ActionResult AuthorInfo()
        {
            var data = db.Authors;
            return View(data);
        }
        [HttpPost]
        public JsonResult AuthorRegistration(Author author)
        {
            try
            {
                db.Authors.Add(author);
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

        [HttpPost]

        public JsonResult AuthorDelete(int id)
        {
            try
            {
                var data = db.Authors.Find(id);
                db.Authors.Remove(data);
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

        public JsonResult AuthorUpdate(Author author)
        {
            try
            {
                Author data = db.Authors.Find(author.author_id);


                data.author_firstname = author.author_firstname;
                data.author_lastname = author.author_lastname;
                data.author_description = author.author_description;
                db.SaveChanges();
                return Json(true, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {

                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }
    }
}