using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Site.Models;

namespace Site.Controllers
{
    public class NewspaperController : Controller
    {
        LibraryMISEntities db = new LibraryMISEntities();
        // GET: Newspaper
        public ActionResult NewspaperRegistrationView()
        {
            return View();
        }

        public JsonResult NewspaperRegistration(Newspaper newspaper)
        {
            try
            {
                db.Newspapers.Add(newspaper);
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

        public ActionResult NewspaperView()
        {
            var data = db.Newspapers;
            return View(data);
        }

        public JsonResult NewspaperDelete(long id)
        {
            try
            {
                Newspaper data = db.Newspapers.Find(id);
                db.Newspapers.Remove(data);
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

        public ActionResult NewspaperEditView(long id)
        {
            var data = db.Newspapers.Find(id);
            return View(data);
        }

        public JsonResult NewspaperEdit(Newspaper newspaper)
        {
            try
            {
                Newspaper data = db.Newspapers.Find(newspaper.newspaper_id);
                data.newspaper_name = newspaper.newspaper_name;
                data.publisher_name = newspaper.publisher_name;
                data.quantity = newspaper.quantity;
                data.date_type = newspaper.date_type;
                data.publication_date = newspaper.publication_date;
                data.registration_date = newspaper.registration_date;
                int res = db.SaveChanges();
                if (res != 0)
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
    }
}